using Cysharp.Threading.Tasks;
using Zenject;

namespace Infrastructure.StateMachines.InitializationStateMachine
{
    public class BaseInitializationState : IState
    {
        protected readonly InitializationStateMachine _stateMachine;

        [Inject]
        protected BaseInitializationState(InitializationStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual UniTask Exit()
        {
            return default;
        }
    }
}