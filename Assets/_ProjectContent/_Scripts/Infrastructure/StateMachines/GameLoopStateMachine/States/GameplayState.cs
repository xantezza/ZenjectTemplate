using Infrastructure.Services.Saving;
using Infrastructure.StateMachines.StateMachine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class GameplayState : BaseState, IEnterableState
    {
        private readonly GameLoopStateMachine _gameLoopStateMachine;
        private readonly ISaveService _saveService;

        public override string StateName => nameof(GameplayState);

        [Inject]
        public GameplayState(GameLoopStateMachine gameLoopStateMachine, ISaveService saveService)
        {
            _saveService = saveService;
            _gameLoopStateMachine = gameLoopStateMachine;
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
            _saveService.StoreAllSaveData(false);
        }
    }
}