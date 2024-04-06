using Cysharp.Threading.Tasks;
using Infrastructure.Services.Saving;
using Infrastructure.StateMachines.StateMachine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class GameplayState : BaseGameLoopState, IEnterableState
    {
        private readonly ISaveService _saveService;

        [Inject]
        public GameplayState(GameLoopStateMachine gameLoopStateMachine, ISaveService saveService) : base(gameLoopStateMachine)
        {
            _saveService = saveService;
        }

        //State changes by GameLoopStateSwitchButton in scene
        public UniTask Enter()
        {
            return default;
        }
    }
}