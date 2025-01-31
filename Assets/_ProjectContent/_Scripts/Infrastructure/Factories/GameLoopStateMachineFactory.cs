using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.GameLoopStateMachine;
using JetBrains.Annotations;
using Zenject;

namespace Infrastructure.Factories
{
    [UsedImplicitly]
    public class GameLoopStateMachineFactory : IGameLoopStateMachineFactory
    {
        private GameLoopStateMachine _stateMachine;
        private readonly IConditionalLoggingService _loggingService;
        private readonly IInstantiator _instantiator;

        [Inject]
        public GameLoopStateMachineFactory(IInstantiator instantiator, IConditionalLoggingService loggingService)
        {
            _instantiator = instantiator;
            _loggingService = loggingService;
        }

        public GameLoopStateMachine GetFrom(object summoner)
        {
            _stateMachine ??= _instantiator.Instantiate<GameLoopStateMachine>();
            _loggingService.Log($"Access to the {this} is obtained from {summoner}", LogTag.GameLoopStateMachine);
            return _stateMachine;
        }
    }
}