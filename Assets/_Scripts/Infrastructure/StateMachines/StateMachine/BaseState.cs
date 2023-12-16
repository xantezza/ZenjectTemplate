namespace Infrastructure.StateMachines.StateMachine
{
    public abstract class BaseState
    {
        public abstract string StateName { get; }

        public virtual void Exit()
        {
        }
    }
}