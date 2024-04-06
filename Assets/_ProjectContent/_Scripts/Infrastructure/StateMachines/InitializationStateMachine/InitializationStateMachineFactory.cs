using Infrastructure.Services.Logging;
using JetBrains.Annotations;
using Zenject;

namespace Infrastructure.StateMachines.InitializationStateMachine
{
    [UsedImplicitly]
    public class InitializationStateMachineFactory
    {
        private InitializationStateMachine _stateMachine;
        private readonly ConditionalLoggingService _loggingService;
        private readonly IInstantiator _instantiator;

        [Inject]
        public InitializationStateMachineFactory(IInstantiator instantiator, ConditionalLoggingService loggingService)
        {
            _instantiator = instantiator;
            _loggingService = loggingService;
        }

        public InitializationStateMachine GetFrom(object summoner)
        {
            _stateMachine ??= _instantiator.Instantiate<InitializationStateMachine>();
            _loggingService.Log($"Access to the GameLoopStateMachine is obtained from {summoner}", LogTag.InitializationStateMachine);
            return _stateMachine;
        }
    }
}