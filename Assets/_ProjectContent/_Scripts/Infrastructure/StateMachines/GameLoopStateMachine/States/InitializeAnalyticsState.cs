using Cysharp.Threading.Tasks;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class InitializeAnalyticsState : BaseGameLoopState, IEnterableState
    {
        private readonly IConditionalLoggingService _conditionalLoggingService;
        private readonly IAnalyticsLogService _analyticsLogService;

        [Inject]
        public InitializeAnalyticsState(
            GameLoopStateMachine stateMachine,
            IConditionalLoggingService conditionalLoggingService,
            IAnalyticsLogService analyticsLogService
        ) : base(stateMachine)
        {
            _analyticsLogService = analyticsLogService;
            _conditionalLoggingService = conditionalLoggingService;
        }

        private void ToNextState()
        {
            _gameLoopStateMachine.Enter<InitializeSaveServiceState>();
        }

        public async UniTask Enter()
        {
            await _analyticsLogService.Initialize();
            ToNextState();
        }
    }
}