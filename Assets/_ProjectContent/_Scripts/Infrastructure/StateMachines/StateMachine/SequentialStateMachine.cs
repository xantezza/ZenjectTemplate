using Cysharp.Threading.Tasks;
using Infrastructure.Services.Logging;

namespace Infrastructure.StateMachines.StateMachine
{
    public abstract class SequentialStateMachine : BaseStateMachine
    {
        protected SequentialStateMachine(IConditionalLoggingService conditionalLoggingService) : base(conditionalLoggingService)
        {
        }

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