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
        private readonly IConditionalLoggingService _conditionalLoggingService;

        [Inject]
        public InitializeDefaultConfigState(
            InitializationStateMachine gameLoopStateMachine,
            IDefaultConfigProvider defaultConfigProvider,
            IConditionalLoggingService conditionalLoggingService
        ) : base(gameLoopStateMachine)
        {
            _conditionalLoggingService = conditionalLoggingService;
            _defaultConfigProvider = defaultConfigProvider;
        }

        public async UniTask Enter()
        {
            RemoteConfig.InitializeByDefault(_defaultConfigProvider.CachedConfig, _conditionalLoggingService);
            await _stateMachine.NextState();
        }
    }
}