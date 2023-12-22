using Infrastructure.Providers;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        private GameLoopStateMachineProvider _stateMachineProvider;

        public static bool IsAwakened { get; private set; }

        [Inject]
        private void Inject(GameLoopStateMachineProvider stateMachineProvider)
        {
            _stateMachineProvider = stateMachineProvider;
        }

        private void Start()
        {
            IsAwakened = true;
            _stateMachineProvider.StateMachine(this).Enter<EntryPointState>();
        }
    }
}