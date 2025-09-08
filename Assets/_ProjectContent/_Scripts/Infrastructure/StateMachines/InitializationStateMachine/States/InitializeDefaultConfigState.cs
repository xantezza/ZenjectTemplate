using Configs;
using Configs.RemoteConfig;
using Cysharp.Threading.Tasks;
using Infrastructure.Providers.DefaultConfigProvider;
using Infrastructure.Services.Log;
using Zenject;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    public class InitializeDefaultConfigState : BaseInitializationState, IEnterableState
    {
        private readonly IDefaultConfigProvider _defaultConfigProvider;

        [Inject]
        public InitializeDefaultConfigState(
            InitializationStateMachine gameLoopStateMachine,
            IDefaultConfigProvider defaultConfigProvider
        ) : base(gameLoopStateMachine)
        {
            _defaultConfigProvider = defaultConfigProvider;
        }

        public async UniTask Enter()
        {
            RemoteConfig.InitializeByDefault(_defaultConfigProvider.CachedConfig);
            await _stateMachine.NextState();
        }
    }
}