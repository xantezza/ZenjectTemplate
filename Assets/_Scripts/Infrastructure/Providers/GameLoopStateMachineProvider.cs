using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.GameLoopStateMachine;
using JetBrains.Annotations;
using Zenject;

namespace Infrastructure.Providers
{
    [UsedImplicitly]
    public class GameLoopStateMachineProvider
    {
        private readonly GameLoopStateMachine _stateMachine;
        private readonly IConditionalLoggingService _loggingService;

        [Inject]
        public GameLoopStateMachineProvider(IInstantiator instantiator, IConditionalLoggingService loggingService)
        {
            _stateMachine = instantiator.Instantiate<GameLoopStateMachine>();
            _loggingService = loggingService;
        }

        public GameLoopStateMachine StateMachine(object summoner)
        {
            _loggingService.Log($"Access to the GameLoopStateMachine is obtained from {summoner}", LogTag.GameLoopStateMachine);
            return _stateMachine;
        }
    }
}