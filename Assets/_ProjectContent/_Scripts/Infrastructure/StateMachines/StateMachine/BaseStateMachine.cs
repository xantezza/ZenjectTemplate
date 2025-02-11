using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Logging;
using Zenject;

namespace Infrastructure.StateMachines.StateMachine
{
    public abstract class BaseStateMachine
    {
        private readonly IConditionalLoggingService _conditionalLoggingService;

        protected IState _activeState;
        private readonly Dictionary<Type, IState> _states = new();
        protected readonly List<IState> _statesList = new();

        protected abstract LogTag LogTag { get; }

        [Inject]
        protected BaseStateMachine(IConditionalLoggingService conditionalLoggingService)
        {
            _conditionalLoggingService = conditionalLoggingService;
        }

        protected async UniTask Enter<TState>() where TState : class, IState, IEnterableState
        {
            var state = await ChangeState<TState>();

            _conditionalLoggingService.Log($"Entering state {typeof(TState).Name}", LogTag);

            await state.Enter();
        }

        protected async UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IState, IPayloadedState<TPayload>
        {
            var state = await ChangeState<TState>();

            _conditionalLoggingService.Log($"Entering state {typeof(TState).Name} with payload: \n{typeof(TPayload).Name}: {payload}", LogTag);

            await state.Enter(payload);
        }
        
        protected async UniTask Enter(int index)
        {
            var state = await ChangeState(index);

            _conditionalLoggingService.Log($"Entering state {state.GetType().Name}", LogTag);

            if (state is IEnterableState enterableState) await enterableState.Enter();
        }

        protected void RegisterState<TState>(TState state) where TState : IState
        {
            _states.Add(typeof(TState), state);
            _statesList.Add(state);
        }

        private async UniTask<TState> ChangeState<TState>() where TState : class, IState
        {
            if (_activeState != null) await _activeState.Exit();

            var state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private async UniTask<IState> ChangeState(int index)
        {
            if (_activeState != null) await _activeState.Exit();

            var state = GetState(index);
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IState
        {
            return _states[typeof(TState)] as TState;
        }

        private IState GetState(int index)
        {
            return _statesList[index];
        }
    }
}