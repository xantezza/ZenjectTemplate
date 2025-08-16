using Configs;
using Configs.RemoteConfig;
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
    public class InitializeUnityServicesState : BaseInitializationState, IEnterableState
    {
        public static bool IsInitialized { get; private set; }

        private readonly LoggingService _loggingService;

        [Inject]
        public InitializeUnityServicesState(
            InitializationStateMachine stateMachine,
            LoggingService loggingService) : base(stateMachine)
        {
            _loggingService = loggingService;
        }

        public async UniTask Enter()
        {
            await InitializeRemoteSettings();
        }

        private async UniTask InitializeRemoteSettings()
        {
            if (IsInitialized)
            {
                await ToNextState();
                return;
            }

#if PLATFORM_WEBGL && !UNITY_EDITOR
            await InitializeUnityServices();
#else
            if (Utilities.CheckForInternetConnection())
            {
                await InitializeUnityServices();
            }
            else
            {
                await ToNextState();
                return;
            }
#endif

            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
            _loggingService.Log("Fetch Configs", LogTag.UnityServices);
            RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
        }

        private async UniTask InitializeUnityServices()
        {
#if DEV
            var options = new InitializationOptions().SetEnvironmentName("production");
            _loggingService.Log("UnityServices.InitializeAsync with environment: production", LogTag.UnityServices);

            //You can uncomment it and add dev environment in dashboard to split production and dev configs if needed
            //var options = new InitializationOptions().SetEnvironmentName("dev");
            //_conditionalLoggingService.Log("UnityServices.InitializeAsync with environment: dev", LogTag.RemoteSettings);
#else
            var options = new InitializationOptions().SetEnvironmentName("production");
#endif

            await UnityServices.InitializeAsync(options);

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                _loggingService.Log("Start of SignInAnonymouslyAsync", LogTag.UnityServices);
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }

        private async void ApplyRemoteSettings(ConfigResponse configResponse)
        {
            _loggingService.Log($"Request Origin: {configResponse.requestOrigin}", LogTag.UnityServices);

            if (configResponse.requestOrigin == ConfigOrigin.Default) return;

            RemoteConfig.InitializeByRemote(RemoteConfigService.Instance.appConfig.config, _loggingService);

            IsInitialized = true;

            await ToNextState();
        }

        private async UniTask ToNextState()
        {
            await _stateMachine.NextState();
        }

        private struct userAttributes
        {
        }

        private struct appAttributes
        {
        }
    }
}