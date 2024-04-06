using Cysharp.Threading.Tasks;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines.GameLoopStateMachine;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using Infrastructure.StateMachines.StateMachine;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    public class InitializationFinalizerState : BaseInitializationState, IEnterableState
    {
        private readonly GameLoopStateMachineFactory _gameLoopStateMachineFactory;
        private readonly IAnalyticsSendService _analyticsSendService;

        protected InitializationFinalizerState(
            InitializationStateMachine stateMachine,
            GameLoopStateMachineFactory gameLoopStateMachineFactory,
            IAnalyticsSendService analyticsSendService) : base(stateMachine)
        {
            _analyticsSendService = analyticsSendService;
            _gameLoopStateMachineFactory = gameLoopStateMachineFactory;
        }

        public async UniTask Enter()
        {
            _analyticsSendService.SendEvent("load_finished");
            await _gameLoopStateMachineFactory.GetFrom(this).Enter<LoadingScreenState, SceneNames>(SceneNames.Menu);
        }
    }
}