using Cysharp.Threading.Tasks;
using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class EntryPointState : BaseGameLoopState, IEnterableState
    {
        [Inject]
        public EntryPointState(GameLoopStateMachine stateMachine) : base(stateMachine)
        {
        }

        private void ToNextState()
        {
            _gameLoopStateMachine.Enter<InitializeDefaultConfigState>();
        }

        public UniTask Enter()
        {
            ToNextState();
            return default;
        }
    }
}