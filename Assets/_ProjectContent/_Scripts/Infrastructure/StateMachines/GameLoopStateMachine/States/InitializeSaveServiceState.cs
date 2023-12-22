using Infrastructure.Services.Saving;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines.StateMachine;
using JetBrains.Annotations;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    [UsedImplicitly]
    public class InitializeSaveServiceState : BaseState, IEnterableState, IPayloadedState<string>
    {
        private readonly ISaveService _saveService;
        private readonly GameLoopStateMachine _gameLoopStateMachine;

        public override string StateName => nameof(InitializeSaveServiceState);

        public InitializeSaveServiceState(GameLoopStateMachine gameLoopStateMachine, ISaveService saveService)
        {
            _gameLoopStateMachine = gameLoopStateMachine;
            _saveService = saveService;
        }

        private void ToNextState(SceneNames nextScene)
        {
            _gameLoopStateMachine.Enter<LoadSceneState, SceneNames>(nextScene);
        }

        public void Enter()
        {
            _saveService.LoadAllData();
            ToNextState(SceneNames.Menu);
        }

        public void Enter(string saveName)
        {
            _saveService.LoadAllData(false, saveName);
            ToNextState(SceneNames.Gameplay);
        }

        public override void Exit()
        {
        }
    }
}