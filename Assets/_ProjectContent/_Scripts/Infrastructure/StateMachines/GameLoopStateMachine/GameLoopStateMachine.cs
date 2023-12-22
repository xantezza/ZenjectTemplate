using System.Collections.Generic;
using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using Infrastructure.StateMachines.StateMachine;
using JetBrains.Annotations;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine
{
    [UsedImplicitly]
    public class GameLoopStateMachine : BaseStateMachine
    {
        protected override LogTag LogTag => LogTag.GameLoopStateMachine;

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