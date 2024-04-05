using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines
{
    public class StatesFactory
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