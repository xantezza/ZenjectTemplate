using System.Collections.Generic;
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
        public GameLoopStateMachine(IInstantiator instantiator, IConditionalLoggingService conditionalLoggingService) : base(conditionalLoggingService)
        {
            var stateMachine = this;
            var extraArgs = new List<object> {stateMachine};

            RegisterState(instantiator.Instantiate<EntryPointState>(extraArgs));
            RegisterState(instantiator.Instantiate<InitializeDefaultConfigState>(extraArgs));
            RegisterState(instantiator.Instantiate<LoadSceneState>(extraArgs));
            RegisterState(instantiator.Instantiate<InitializeRemoteConfigState>(extraArgs));
            RegisterState(instantiator.Instantiate<InitializeDebugState>(extraArgs));
            RegisterState(instantiator.Instantiate<InitializeAnalyticsState>(extraArgs));
            RegisterState(instantiator.Instantiate<InitializeSaveServiceState>(extraArgs));
            RegisterState(instantiator.Instantiate<MenuState>(extraArgs));
            RegisterState(instantiator.Instantiate<GameplayState>(extraArgs));
        }
    }
}