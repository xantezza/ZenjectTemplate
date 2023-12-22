using Cysharp.Threading.Tasks;
using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class InitializeAnalyticsState : BaseGameLoopState, IEnterableState
    {
        public override string StateName => nameof(InitializeAnalyticsState);
        
        [Inject]
        public InitializeAnalyticsState(GameLoopStateMachine stateMachine) : base(stateMachine)
        {
        }

        private void ToNextState()
        {
            _gameLoopStateMachine.Enter<InitializeSaveServiceState>();
        }

        public async void Enter()
        {
            await InitializeAnalytics();
        }

        private async UniTask InitializeAnalytics()
        {
            //placeholder
            await UniTask.Delay(2000);
            ToNextState();
        }
    }
}