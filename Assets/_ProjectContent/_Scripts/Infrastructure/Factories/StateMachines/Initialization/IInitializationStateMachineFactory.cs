using Infrastructure.StateMachines.InitializationStateMachine;

namespace Infrastructure.Factories.StateMachines.Initialization
{
    public interface IInitializationStateMachineFactory
    {
        InitializationStateMachine GetFrom(object summoner);
    }
}