namespace Infrastructure.StateMachines.StateMachine
{
    public abstract class BaseState
    {
        public abstract string StateName { get; }

        /// <summary>
        /// Empty by default
        /// </summary>
        public virtual void Exit() { }
    }
}