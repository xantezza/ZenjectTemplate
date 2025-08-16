using Cysharp.Threading.Tasks;
using Infrastructure.Factories;
using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine
{
    public class GameLoopStateMachine : BaseStateMachine
    {
        protected override LogTag LogTag => LogTag.GameLoopStateMachine;

        [Inject]
        public GameLoopStateMachine(IStatesFactory statesFactory, LoggingService loggingService) : base(loggingService)
        {
            RegisterState(statesFactory.Create<EntryPointState>(this));
            RegisterState(statesFactory.Create<MenuState>(this));
            RegisterState(statesFactory.Create<GameplayState>(this));
        }
        public new async UniTask Enter<TState>() where TState : class, IState, IEnterableState
        {
            await base.Enter<TState>();
        }
    }
}