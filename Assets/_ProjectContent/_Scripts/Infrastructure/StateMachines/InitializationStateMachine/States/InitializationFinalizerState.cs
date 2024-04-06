using Cysharp.Threading.Tasks;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines.GameLoopStateMachine;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using Infrastructure.StateMachines.StateMachine;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    public class InitializationFinalizerState : BaseInitializationState, IEnterableState
    {
        private readonly GameLoopStateMachineFactory _gameLoopStateMachineFactory;

        protected InitializationFinalizerState(InitializationStateMachine stateMachine, GameLoopStateMachineFactory gameLoopStateMachineFactory) : base(stateMachine)
        {
            _gameLoopStateMachineFactory = gameLoopStateMachineFactory;
        }

        public async UniTask Enter()
        {
            await _gameLoopStateMachineFactory.GetFrom(this).Enter<LoadingScreenState, SceneNames>(SceneNames.Menu);
        }
    }
}