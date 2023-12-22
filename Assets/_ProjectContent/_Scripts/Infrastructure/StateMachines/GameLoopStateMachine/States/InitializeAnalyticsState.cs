using System;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
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

        public override string StateName => nameof(InitializeAnalyticsState);

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

        public async void Enter()
        {
            await _analyticsLogService.Initialize();
            ToNextState();
        }
    }
}