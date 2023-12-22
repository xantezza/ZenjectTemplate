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

        public override string StateName => nameof(InitializeSaveServiceState);

        public InitializeSaveServiceState(GameLoopStateMachine gameLoopStateMachine, ISaveService saveService) : base(gameLoopStateMachine)
        {
            _saveService = saveService;
        }

        private void ToNextState(SceneNames nextScene)
        {
            _gameLoopStateMachine.Enter<LoadSceneState, SceneNames>(nextScene);
        }

        public void Enter()
        {
            _saveService.LoadSaveFile();
            ToNextState(SceneNames.Menu);
        }

        public void Enter(string saveName)
        {
            _saveService.LoadSaveFile(false, saveName);
            ToNextState(SceneNames.Gameplay);
        }

        public override void Exit()
        {
        }
    }
}