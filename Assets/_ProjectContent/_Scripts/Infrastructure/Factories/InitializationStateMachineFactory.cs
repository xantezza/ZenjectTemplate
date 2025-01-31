using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.InitializationStateMachine;
using JetBrains.Annotations;
using Zenject;

namespace Infrastructure.Factories
{
    [UsedImplicitly]
    public class InitializationStateMachineFactory : IInitializationStateMachineFactory
    {
        private InitializationStateMachine _stateMachine;
        private readonly IConditionalLoggingService _loggingService;
        private readonly IInstantiator _instantiator;

        [Inject]
        public InitializationStateMachineFactory(IInstantiator instantiator, IConditionalLoggingService loggingService)
        {
            _instantiator = instantiator;
            _loggingService = loggingService;
        }

        public InitializationStateMachine GetFrom(object summoner)
        {
            _stateMachine ??= _instantiator.Instantiate<InitializationStateMachine>();
            _loggingService.Log($"Access to the {this} is obtained from {summoner}", LogTag.InitializationStateMachine);
            return _stateMachine;
        }
    }
}