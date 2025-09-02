using Cysharp.Threading.Tasks;

namespace Infrastructure.StateMachines
{
    public interface IState
    {
        public UniTask Exit();
    }
}