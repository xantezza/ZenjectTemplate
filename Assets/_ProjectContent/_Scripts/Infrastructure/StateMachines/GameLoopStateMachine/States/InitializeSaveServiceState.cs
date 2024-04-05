using Cysharp.Threading.Tasks;
using Infrastructure.Services.Saving;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines.StateMachine;
using JetBrains.Annotations;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    [UsedImplicitly]
    public class InitializeSaveServiceState : BaseGameLoopState, IEnterableState, IPayloadedState<string>
    {
        private readonly ISaveService _saveService;
        
        public InitializeSaveServiceState(GameLoopStateMachine gameLoopStateMachine, ISaveService saveService) : base(gameLoopStateMachine)
        {
            _saveService = saveService;
        }

        private void ToNextState(SceneNames nextScene)
        {
            _gameLoopStateMachine.Enter<LoadSceneState, SceneNames>(nextScene);
        }

        public UniTask Enter()
        {
            _saveService.LoadSaveFile();
            ToNextState(SceneNames.Menu);
            return default;
        }

        public UniTask Enter(string saveName)
        {
            _saveService.LoadSaveFile(false, saveName);
            ToNextState(SceneNames.Gameplay);
            return default;
        }
    }
}