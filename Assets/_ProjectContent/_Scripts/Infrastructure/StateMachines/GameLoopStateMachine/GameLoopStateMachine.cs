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
        public GameLoopStateMachine(StatesFactory statesFactory, ConditionalLoggingService conditionalLoggingService) : base(conditionalLoggingService)
        {
            RegisterState(statesFactory.Create<EntryPointState>(this));
            RegisterState(statesFactory.Create<LoadingScreenState>(this));
            RegisterState(statesFactory.Create<MenuState>(this));
            RegisterState(statesFactory.Create<GameplayState>(this));
        }
    }
}