using Cysharp.Threading.Tasks;

namespace Infrastructure.StateMachines.StateMachine
{
    public interface IEnterableState
    {
        UniTask Enter();
    }
}