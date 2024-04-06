using Configs;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.StateMachine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.RemoteConfig;
using Zenject;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    public class InitializeRemoteConfigState : BaseInitializationState, IEnterableState
    {
        private readonly IConditionalLoggingService _conditionalLoggingService;

        private bool _isInitialized;
        
        [Inject]
        public InitializeRemoteConfigState(
            InitializationStateMachine stateMachine,
            IConditionalLoggingService conditionalLoggingService) : base(stateMachine)
        {
            _conditionalLoggingService = conditionalLoggingService;
        }

        private async UniTask ToNextState()
        {
            await _stateMachine.NextState();
        }

        public async UniTask Enter()
        {
            await InitializeRemoteSettings();
        }

        private async UniTask InitializeRemoteSettings()
        {
            if (_isInitialized)
            {
                await ToNextState();
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
#endif

            await UnityServices.InitializeAsync(options);

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                _conditionalLoggingService.Log("Start of SignInAnonymouslyAsync", LogTag.RemoteSettings);
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        private async void ApplyRemoteSettings(ConfigResponse configResponse)
        {
            _conditionalLoggingService.Log($"Request Origin: {configResponse.requestOrigin}", LogTag.RemoteSettings);

            if (configResponse.requestOrigin == ConfigOrigin.Default) return;

            Remote.InitializeByRemote(RemoteConfigService.Instance.appConfig.config, _conditionalLoggingService);

            _isInitialized = true;

            await ToNextState();
        }

        private struct userAttributes
        {
        }

        private struct appAttributes
        {
        }
    }
}