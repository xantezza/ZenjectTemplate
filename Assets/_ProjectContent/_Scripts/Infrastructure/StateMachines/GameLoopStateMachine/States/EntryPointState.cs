using Cysharp.Threading.Tasks;
using Infrastructure.Factories;
using Infrastructure.Providers;
using Infrastructure.Services.LoadingCurtain;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class EntryPointState : BaseGameLoopState, IEnterableState
    {
        private readonly IInitializationStateMachineFactory _initializationStateMachineFactory;
        private readonly ILoadingCurtainService _loadingCurtainService;

        [Inject]
        public EntryPointState(
            GameLoopStateMachine stateMachine, 
            IInitializationStateMachineFactory initializationStateMachineFactory, 
            ILoadingCurtainService loadingCurtainService
            ) : base(stateMachine)
        {
            _loadingCurtainService = loadingCurtainService;
            _initializationStateMachineFactory = initializationStateMachineFactory;
        }

        public async UniTask Enter()
        {
            _loadingCurtainService.ForceShow();
            await ToNextState();
        }

        private async UniTask ToNextState()
        {
            await _initializationStateMachineFactory.GetFrom(this).NextState();
        }
    }
}