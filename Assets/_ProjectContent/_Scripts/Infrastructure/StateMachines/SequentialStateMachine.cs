using Cysharp.Threading.Tasks;

namespace Infrastructure.StateMachines
{
    public abstract class SequentialStateMachine : BaseStateMachine
    {
        public async UniTask NextState()
        {
            var indexOfActiveState = -1;
            if (_activeState != null)
            {
                indexOfActiveState = _statesList.IndexOf(_activeState);
                if (indexOfActiveState == _statesList.Count - 1)
                {
                    await _activeState.Exit();
                    _activeState = null;
                    return;
                }
            }

            await Enter(indexOfActiveState + 1);
        }
    }
}