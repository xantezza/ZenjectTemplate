using Infrastructure.StateMachines.GameLoopStateMachine;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        private GameLoopStateMachineFactory _stateMachineFactory;

        public static bool HasStarted { get; private set; }

        [Inject]
        private void Inject(GameLoopStateMachineFactory stateMachineFactory)
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