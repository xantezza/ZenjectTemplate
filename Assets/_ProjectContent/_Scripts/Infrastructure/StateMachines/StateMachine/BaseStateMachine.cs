using System;
using System.Collections.Generic;
using Infrastructure.Services.Logging;
using Zenject;

namespace Infrastructure.StateMachines.StateMachine
{
    public abstract class BaseStateMachine
    {
        private readonly IConditionalLoggingService _conditionalLoggingService;

        private IState _activeState;
        private readonly Dictionary<Type, IState> _states;

        protected abstract LogTag LogTag { get; }

        [Inject]
        protected BaseStateMachine(IConditionalLoggingService conditionalLoggingService)
        {
            _states = new Dictionary<Type, IState>();
            _conditionalLoggingService = conditionalLoggingService;
        }

        public void Enter<TState>() where TState : class, IState, IEnterableState
        {
            var state = ChangeState<TState>();

            _conditionalLoggingService.Log($"Entering state {typeof(TState).Name}", LogTag);

            state?.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IState, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();

            _conditionalLoggingService.Log($"Entering state {typeof(TState).Name} with payload: \n{typeof(TPayload).Name}: {payload}", LogTag);

            state?.Enter(payload);
        }

        public void Enter<TState, TPayload, TPayload1>(TPayload payload, TPayload1 payload1) where TState : class, IState, IPayloadedState<TPayload, TPayload1>
        {
            var state = ChangeState<TState>();

            _conditionalLoggingService.Log($"Entering state {typeof(TState).Name} with payload: \n{typeof(TPayload).Name}: {payload} \n{typeof(TPayload1).Name}: {payload1}", LogTag);

            state?.Enter(payload, payload1);
        }

        protected void RegisterState<TState>(TState state) where TState : IState
        {
            _states.Add(typeof(TState), state);
        }

        private TState ChangeState<TState>() where TState : class, IState
        {
            _activeState?.Exit();

            var state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}