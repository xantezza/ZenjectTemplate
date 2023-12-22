using Infrastructure.Services.Saving;
using Infrastructure.StateMachines.StateMachine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class GameplayState : BaseGameLoopState, IEnterableState
    {
        private readonly ISaveService _saveService;

        public override string StateName => nameof(GameplayState);

        [Inject]
        public GameplayState(GameLoopStateMachine gameLoopStateMachine, ISaveService saveService) : base(gameLoopStateMachine)
        {
            _saveService = saveService;
        }

        //State changes by GameStateSwitchButton in scene
        public void Enter()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene _)
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            _saveService.StoreSaveFile(false);
        }
    }
}