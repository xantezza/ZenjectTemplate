using Infrastructure.StateMachines.GameLoopStateMachine;

namespace Infrastructure.Factories.StateMachines.GameLoop
{
    public interface IGameLoopStateMachineFactory
    {
        GameLoopStateMachine GetFrom(object summoner);
    }
}