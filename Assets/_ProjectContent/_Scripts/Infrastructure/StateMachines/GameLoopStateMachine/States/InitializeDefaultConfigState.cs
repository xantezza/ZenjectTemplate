using System;
using Configs;
using Cysharp.Threading.Tasks;
using Infrastructure.Providers.DefaultConfigProvider;
using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class InitializeDefaultConfigState : BaseGameLoopState, IEnterableState
    {
        private readonly IDefaultConfigProvider _defaultConfigProvider;
        private readonly IConditionalLoggingService _conditionalLoggingService;

        [Inject]
        public InitializeDefaultConfigState(
            GameLoopStateMachine gameLoopStateMachine,
            IDefaultConfigProvider defaultConfigProvider,
            IConditionalLoggingService conditionalLoggingService
        ) : base(gameLoopStateMachine)
        {
            _conditionalLoggingService = conditionalLoggingService;
            _defaultConfigProvider = defaultConfigProvider;
        }

        private void ToNextState()
        {
            _gameLoopStateMachine.Enter<InitializeRemoteConfigState>();
        }

        private void OnLoadingSceneLoadedCallback()
        {
            ToNextState();
        }

        public UniTask Enter()
        {
            Remote.InitializeByDefault(_defaultConfigProvider.CachedConfig, _conditionalLoggingService);

            _gameLoopStateMachine.Enter<LoadSceneState, Action>(OnLoadingSceneLoadedCallback);
            return default;
        }
    }
}