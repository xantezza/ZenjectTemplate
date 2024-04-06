using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.InitializationStateMachine.States;
using Infrastructure.StateMachines.StateMachine;

namespace Infrastructure.StateMachines.InitializationStateMachine
{
    public class InitializationStateMachine : BaseStateMachine
    {
        protected override LogTag LogTag => LogTag.InitializationStateMachine;

        public InitializationStateMachine(StatesFactory statesFactory, IConditionalLoggingService conditionalLoggingService) : base(conditionalLoggingService)
        {
            RegisterState(statesFactory.Create<InitializeDefaultConfigState>(this));
            RegisterState(statesFactory.Create<InitializeRemoteConfigState>(this));
            RegisterState(statesFactory.Create<InitializeDebugState>(this));
            RegisterState(statesFactory.Create<InitializeAnalyticsState>(this));
            RegisterState(statesFactory.Create<InitializeSaveServiceState>(this));
            RegisterState(statesFactory.Create<InitializationFinalizerState>(this));
        }
    }
}