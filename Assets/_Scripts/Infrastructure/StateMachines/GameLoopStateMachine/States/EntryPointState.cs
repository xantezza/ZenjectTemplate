using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class EntryPointState : BaseState, IEnterableState
    {
        private readonly GameLoopStateMachine _gameLoopStateMachine;
        public override string StateName => nameof(EntryPointState);

        [Inject]
        public EntryPointState(GameLoopStateMachine gameLoopStateMachine)
        {
            _gameLoopStateMachine = gameLoopStateMachine;
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