using Infrastructure.StateMachines.StateMachine;
using JetBrains.Annotations;
using Zenject;

namespace Infrastructure.Factories
{
    [UsedImplicitly]
    public class StatesFactory : IStatesFactory
    {
        private readonly IInstantiator _instantiator;

        public StatesFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public TState Create<TState>(BaseStateMachine stateMachine) where TState : class, IState
        {
            return _instantiator.Instantiate<TState>(new object[] {stateMachine});
        }
    }
}