using Cysharp.Threading.Tasks;
using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class MenuState : BaseGameLoopState, IEnterableState
    {
        [Inject]
        public MenuState(GameLoopStateMachine stateMachine) : base(stateMachine)
        {
        }

        //State changes by GameLoopStateSwitchButton in scene
        public UniTask Enter()
        {
            return default;
        }
    }
}