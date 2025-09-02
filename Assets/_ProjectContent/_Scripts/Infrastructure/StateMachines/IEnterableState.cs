using Cysharp.Threading.Tasks;

namespace Infrastructure.StateMachines
{
    public interface IEnterableState
    {
        UniTask Enter();
    }
}