using Infrastructure.Factories;
using Infrastructure.StateMachines.GameLoopStateMachine;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        private IGameLoopStateMachineFactory _stateMachineFactory;

        public static bool HasStarted { get; private set; }

        [Inject]
        private void Inject(IGameLoopStateMachineFactory stateMachineFactory)
        {
            _stateMachineFactory = stateMachineFactory;
        }

        private async void Start()
        {
            HasStarted = true;
            await _stateMachineFactory.GetFrom(this).Enter<EntryPointState>();
        }
    }
}