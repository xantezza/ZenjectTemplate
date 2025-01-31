using Infrastructure.StateMachines.GameLoopStateMachine;

namespace Infrastructure.Factories
{
    public interface IGameLoopStateMachineFactory
    {
        GameLoopStateMachine GetFrom(object summoner);
    }
}