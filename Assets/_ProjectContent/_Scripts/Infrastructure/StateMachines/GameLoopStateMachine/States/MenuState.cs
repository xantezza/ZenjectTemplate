using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class MenuState : BaseGameLoopState, IEnterableState
    {
        public override string StateName => nameof(MenuState);

        [Inject]
        public MenuState(GameLoopStateMachine stateMachine) : base(stateMachine)
        {
        }

        //State changes by GameStateSwitchButton in scene
        public void Enter()
        {
        }
    }
}