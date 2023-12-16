using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class MenuState : BaseState, IEnterableState
    {
        private readonly GameLoopStateMachine _stateMachine;
        public override string StateName => nameof(MenuState);

        [Inject]
        public MenuState(GameLoopStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        //State changes by GameStateSwitchButton in scene
        public void Enter()
        {
        }
    }
}