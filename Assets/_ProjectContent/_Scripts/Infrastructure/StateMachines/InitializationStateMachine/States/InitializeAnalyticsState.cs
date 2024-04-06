using Cysharp.Threading.Tasks;
using Infrastructure.Services.Analytics;
using Infrastructure.StateMachines.StateMachine;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using Zenject;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    public class InitializeAnalyticsState : BaseInitializationState, IEnterableState
    {
        private readonly IAnalyticsLogService _analyticsLogService;

        [Inject]
        public InitializeAnalyticsState(
            InitializationStateMachine stateMachine,
            IAnalyticsLogService analyticsLogService
        ) : base(stateMachine)
        {
            _analyticsLogService = analyticsLogService;
        }

        private async UniTask ToNextState()
        {
            await _stateMachine.NextState();
        }

        public async UniTask Enter()
        {
            await _analyticsLogService.Initialize();
            await ToNextState();
        }
    }
}