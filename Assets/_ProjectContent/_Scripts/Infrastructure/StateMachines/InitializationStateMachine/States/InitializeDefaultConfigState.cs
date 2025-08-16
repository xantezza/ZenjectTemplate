using Configs;
using Configs.RemoteConfig;
using Cysharp.Threading.Tasks;
using Infrastructure.Providers.DefaultConfigProvider;
using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    public class InitializeDefaultConfigState : BaseInitializationState, IEnterableState
    {
        private readonly IDefaultConfigProvider _defaultConfigProvider;
        private readonly LoggingService _loggingService;

        [Inject]
        public InitializeDefaultConfigState(
            InitializationStateMachine gameLoopStateMachine,
            IDefaultConfigProvider defaultConfigProvider,
            LoggingService loggingService
        ) : base(gameLoopStateMachine)
        {
            _loggingService = loggingService;
            _defaultConfigProvider = defaultConfigProvider;
        }

        public async UniTask Enter()
        {
            RemoteConfig.InitializeByDefault(_defaultConfigProvider.CachedConfig, _loggingService);
            await _stateMachine.NextState();
        }
    }
}