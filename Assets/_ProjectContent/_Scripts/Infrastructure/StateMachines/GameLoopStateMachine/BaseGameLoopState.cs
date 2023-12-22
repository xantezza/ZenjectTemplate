using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine
{
    public abstract class BaseGameLoopState : BaseState
    {
        protected readonly GameLoopStateMachine _gameLoopStateMachine;

        [Inject]
        protected BaseGameLoopState(GameLoopStateMachine stateMachine)
        {
            _gameLoopStateMachine = stateMachine;
        }
    }
}