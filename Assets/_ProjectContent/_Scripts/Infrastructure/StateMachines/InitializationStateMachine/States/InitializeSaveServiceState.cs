using Cysharp.Threading.Tasks;
using Infrastructure.Services.Saving;
using Infrastructure.StateMachines.StateMachine;
using JetBrains.Annotations;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    [UsedImplicitly]
    public class InitializeSaveServiceState : BaseInitializationState, IEnterableState
    {
        private readonly ISaveService _saveService;

        public InitializeSaveServiceState(InitializationStateMachine gameLoopStateMachine, ISaveService saveService) : base(gameLoopStateMachine)
        {
            _saveService = saveService;
        }

        private async UniTask ToNextState()
        {
            await _stateMachine.NextState();
        }

        public async UniTask Enter()
        {
            _saveService.LoadSaveFile();
            await ToNextState();
        }
    }
}