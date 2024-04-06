using Cysharp.Threading.Tasks;
using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine
{
    public abstract class BaseGameLoopState : IState
    {
        protected readonly GameLoopStateMachine _stateMachine;

        [Inject]
        protected BaseGameLoopState(GameLoopStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual UniTask Exit()
        {
            return default;
        }
    }
}