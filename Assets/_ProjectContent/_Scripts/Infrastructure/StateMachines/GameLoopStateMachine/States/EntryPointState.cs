using Cysharp.Threading.Tasks;
using Infrastructure.Factories;
using Infrastructure.Providers;
using Infrastructure.Providers.LoadingCurtainProvider;
using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class EntryPointState : BaseGameLoopState, IEnterableState
    {
        private readonly IInitializationStateMachineFactory _initializationStateMachineFactory;
        private readonly ILoadingCurtainProvider _loadingCurtainProvider;

        [Inject]
        public EntryPointState(
            GameLoopStateMachine stateMachine, 
            IInitializationStateMachineFactory initializationStateMachineFactory, 
            ILoadingCurtainProvider loadingCurtainProvider
            ) : base(stateMachine)
        {
            _loadingCurtainProvider = loadingCurtainProvider;
            _initializationStateMachineFactory = initializationStateMachineFactory;
        }

        public async UniTask Enter()
        {
            _loadingCurtainProvider.ForceShow();
            await ToNextState();
        }

        private async UniTask ToNextState()
        {
            await _initializationStateMachineFactory.GetFrom(this).NextState();
        }
    }
}