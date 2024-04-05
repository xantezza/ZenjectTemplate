using Cysharp.Threading.Tasks;

namespace Infrastructure.StateMachines.StateMachine
{
    public interface IPayloadedState<in TPayload>
    {
        UniTask Enter(TPayload payload);
    }

    public interface IPayloadedState<in TPayload, in TPayload1>
    {
        UniTask Enter(TPayload payload, TPayload1 payload1);
    }
}