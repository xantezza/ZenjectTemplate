using Infrastructure.Factories;
using Infrastructure.Factories.StateMachines;
using Infrastructure.Services.Log;
using Infrastructure.StateMachines.InitializationStateMachine.States;
using JetBrains.Annotations;

namespace Infrastructure.StateMachines.InitializationStateMachine
{
    [UsedImplicitly]
    public class InitializationStateMachine : SequentialStateMachine
    {
        protected override LogTag LogTag => LogTag.InitializationStateMachine;

        public InitializationStateMachine(IStatesFactory statesFactory)
        {
#if DEV
            RegisterState(statesFactory.Create<InitializeDebugToolsState>(this));
#else
#endif
            RegisterState(statesFactory.Create<InitializeErrorModalState>(this));
            RegisterState(statesFactory.Create<InitializeDefaultConfigState>(this));
            RegisterState(statesFactory.Create<InitializeUnityServicesState>(this));
            RegisterState(statesFactory.Create<InitializeSaveServiceState>(this));
            RegisterState(statesFactory.Create<InitializePrivacyPolicyModalState>(this));
            RegisterState(statesFactory.Create<InitializationFinalizerState>(this));
        }
    }
}