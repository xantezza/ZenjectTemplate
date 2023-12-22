﻿using Configs;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.StateMachine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.RemoteConfig;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class InitializeRemoteConfigState : BaseState, IEnterableState
    {
        private readonly GameLoopStateMachine _stateMachine;
        private readonly IConditionalLoggingService _conditionalLoggingService;

        private bool _isInitialized;

        public override string StateName => nameof(InitializeRemoteConfigState);

        [Inject]
        public InitializeRemoteConfigState(
            GameLoopStateMachine stateMachine,
            IConditionalLoggingService conditionalLoggingService)
        {
            _stateMachine = stateMachine;
            _conditionalLoggingService = conditionalLoggingService;
        }

        private void ToNextState()
        {
            _stateMachine.Enter<InitializeDebugState>();
        }

        public async void Enter()
        {
            await InitializeRemoteSettings();
        }

        private async UniTask InitializeRemoteSettings()
        {
            if (_isInitialized)
            {
                ToNextState();
                return;
            }

#if PLATFORM_WEBGL && !UNITY_EDITOR
            await InitializeRemoteConfigAsync();
#else
            if (Utilities.CheckForInternetConnection()) await InitializeRemoteConfigAsync();
#endif

            _conditionalLoggingService.Log("Subscribe on fetch", LogTag.RemoteSettings);
            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
            RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
        }

        private async UniTask InitializeRemoteConfigAsync()
        {
#if DEV
            var options = new InitializationOptions().SetEnvironmentName("dev");
            _conditionalLoggingService.Log("Start of InitializeRemoteConfigAsync with environment: dev", LogTag.RemoteSettings);
#else
            var options = new InitializationOptions().SetEnvironmentName("production");
            _loggingService.Log($"Start of InitializeRemoteConfigAsync with environment: production", LogTag.RemoteSettings);
#endif

            await UnityServices.InitializeAsync(options);

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                _conditionalLoggingService.Log("Start of SignInAnonymouslyAsync", LogTag.RemoteSettings);
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        private void ApplyRemoteSettings(ConfigResponse configResponse)
        {
            _conditionalLoggingService.Log($"Request Origin: {configResponse.requestOrigin}", LogTag.RemoteSettings);

            if (configResponse.requestOrigin == ConfigOrigin.Default) return;

            Remote.InitializeByRemote(RemoteConfigService.Instance.appConfig.config, _conditionalLoggingService);

            _isInitialized = true;

            ToNextState();
        }

        private struct userAttributes
        {
        }

        private struct appAttributes
        {
        }
    }
}