using Infrastructure.StateMachines.InitializationStateMachine;

namespace Infrastructure.Factories
{
    public interface IInitializationStateMachineFactory
    {
        InitializationStateMachine GetFrom(object summoner);
    }
}