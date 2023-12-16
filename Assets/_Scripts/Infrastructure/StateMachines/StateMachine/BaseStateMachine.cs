using System;
using System.Collections.Generic;
using Infrastructure.Services.Logging;
using Zenject;

namespace Infrastructure.StateMachines.StateMachine
{
    public abstract class BaseStateMachine
    {
        private readonly IConditionalLoggingService _conditionalLoggingService;

        private BaseState _activeBaseState;
        private readonly Dictionary<Type, BaseState> _states;

        protected abstract LogTag LogTag { get; }

        [Inject]
        protected BaseStateMachine(IConditionalLoggingService conditionalLoggingService)
        {
            _states = new Dictionary<Type, BaseState>();
            _conditionalLoggingService = conditionalLoggingService;
        }

        public void Enter<TState>() where TState : BaseState, IEnterableState
        {
            var state = ChangeState<TState>();

            _conditionalLoggingService.Log($"Entering state {typeof(TState).Name}", LogTag);

            state?.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : BaseState, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();

            _conditionalLoggingService.Log($"Entering state {typeof(TState).Name} with payload: \n{typeof(TPayload).Name}: {payload}", LogTag);

            state?.Enter(payload);
        }

        public void Enter<TState, TPayload, TPayload1>(TPayload payload, TPayload1 payload1) where TState : BaseState, IPayloadedState<TPayload, TPayload1>
        {
            var state = ChangeState<TState>();

            _conditionalLoggingService.Log($"Entering state {typeof(TState).Name} with payload: \n{typeof(TPayload).Name}: {payload} \n{typeof(TPayload1).Name}: {payload1}", LogTag);

            state?.Enter(payload, payload1);
        }

        protected void RegisterState<TState>(TState state) where TState : BaseState
        {
            _states.Add(typeof(TState), state);
        }

        private TState ChangeState<TState>() where TState : BaseState
        {
#if DEV
            var stateName = _activeBaseState == null ? "None" : _activeBaseState?.StateName;
            _conditionalLoggingService.Log($"Exiting state {stateName}", LogTag);
#endif

            _activeBaseState?.Exit();

            var state = GetState<TState>();
            _activeBaseState = state;

            return state;
        }

        private TState GetState<TState>() where TState : BaseState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}