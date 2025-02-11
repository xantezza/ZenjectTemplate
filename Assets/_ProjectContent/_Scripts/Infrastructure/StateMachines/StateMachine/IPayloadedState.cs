using Cysharp.Threading.Tasks;

namespace Infrastructure.StateMachines.StateMachine
{
    public interface IPayloadedState<in TPayload>
    {
        UniTask Enter(TPayload payload);
    }
}