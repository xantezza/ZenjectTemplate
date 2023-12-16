namespace Infrastructure.StateMachines.StateMachine
{
    public interface IPayloadedState<in TPayload>
    {
        void Enter(TPayload payload);
    }

    public interface IPayloadedState<in TPayload, in TPayload1>
    {
        void Enter(TPayload payload, TPayload1 payload1);
    }
}