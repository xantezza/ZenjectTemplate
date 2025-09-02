using Infrastructure.StateMachines;

namespace Infrastructure.Factories
{
    public interface IStatesFactory
    {
        TState Create<TState>(BaseStateMachine stateMachine) where TState : class, IState;
    }
}