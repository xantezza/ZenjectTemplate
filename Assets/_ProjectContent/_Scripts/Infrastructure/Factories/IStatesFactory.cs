using Infrastructure.StateMachines.StateMachine;

namespace Infrastructure.Factories
{
    public interface IStatesFactory
    {
        TState Create<TState>(BaseStateMachine stateMachine) where TState : class, IState;
    }
}