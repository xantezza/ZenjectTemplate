using Infrastructure.Services.Log;
using Infrastructure.StateMachines.GameLoopStateMachine;
using JetBrains.Annotations;
using Zenject;

namespace Infrastructure.Factories
{
    [UsedImplicitly]
    public class GameLoopStateMachineFactory : IGameLoopStateMachineFactory
    {
        private GameLoopStateMachine _stateMachine;
        private readonly IInstantiator _instantiator;

        [Inject]
        public GameLoopStateMachineFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public GameLoopStateMachine GetFrom(object summoner)
        {
            _stateMachine ??= _instantiator.Instantiate<GameLoopStateMachine>();
            Logger.Log($"Access to the {nameof(GameLoopStateMachine)} is obtained from {summoner}", LogTag.GameLoopStateMachine);
            return _stateMachine;
        }
    }
}