using Infrastructure.Services.Log;
using Infrastructure.StateMachines.InitializationStateMachine;
using JetBrains.Annotations;
using Zenject;

namespace Infrastructure.Factories
{
    [UsedImplicitly]
    public class InitializationStateMachineFactory : IInitializationStateMachineFactory
    {
        private InitializationStateMachine _stateMachine;
        private readonly IInstantiator _instantiator;

        [Inject]
        public InitializationStateMachineFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public InitializationStateMachine GetFrom(object summoner)
        {
            _stateMachine ??= _instantiator.Instantiate<InitializationStateMachine>();
            Logger.Log($"Access to the {nameof(InitializationStateMachine)} is obtained from {summoner}", LogTag.InitializationStateMachine);
            return _stateMachine;
        }
    }
}