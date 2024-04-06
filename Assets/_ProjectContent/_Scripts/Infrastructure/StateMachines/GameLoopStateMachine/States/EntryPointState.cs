using System;
using Cysharp.Threading.Tasks;
using Infrastructure.StateMachines.InitializationStateMachine;
using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class EntryPointState : BaseGameLoopState, IEnterableState
    {
        private readonly InitializationStateMachineFactory _initializationStateMachineFactory;

        [Inject]
        public EntryPointState(GameLoopStateMachine stateMachine, InitializationStateMachineFactory initializationStateMachineFactory) : base(stateMachine)
        {
            _initializationStateMachineFactory = initializationStateMachineFactory;
        }

        private async void ToNextState()
        {
            await _initializationStateMachineFactory.GetFrom(this).NextState();
        }

        public async UniTask Enter()
        {
            await _stateMachine.Enter<LoadingScreenState, Action>(ToNextState);
        }
    }
}