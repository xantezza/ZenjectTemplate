using Cysharp.Threading.Tasks;

namespace Infrastructure.StateMachines.StateMachine
{
    public interface IState
    {
        public UniTask Exit();
    }
}