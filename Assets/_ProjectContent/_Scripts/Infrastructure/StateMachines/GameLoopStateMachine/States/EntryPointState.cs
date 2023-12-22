using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class EntryPointState : BaseGameLoopState, IEnterableState
    {
        public override string StateName => nameof(EntryPointState);
        
        [Inject]
        public EntryPointState(GameLoopStateMachine stateMachine) : base(stateMachine)
        {
        }

        private void ToNextState()
        {
            _gameLoopStateMachine.Enter<InitializeDefaultConfigState>();
        }

        public void Enter()
        {
            ToNextState();
        }
    }
}