using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Factories;
using Infrastructure.StateMachines.InitializationStateMachine;
using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class EntryPointState : BaseGameLoopState, IEnterableState
    {
        private readonly IInitializationStateMachineFactory _initializationStateMachineFactory;

        [Inject]
        public EntryPointState(GameLoopStateMachine stateMachine, IInitializationStateMachineFactory initializationStateMachineFactory) : base(stateMachine)
        {
            _initializationStateMachineFactory = initializationStateMachineFactory;
        }

        public async UniTask Enter()
        {
            await _stateMachine.Enter<LoadingScreenState, Action>(ToNextState);
        }

        private async void ToNextState()
        {
            await _initializationStateMachineFactory.GetFrom(this).NextState();
        }
    }
}