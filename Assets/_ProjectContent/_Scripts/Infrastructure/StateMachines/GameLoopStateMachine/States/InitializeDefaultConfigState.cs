﻿using System;
using Configs;
using Infrastructure.Providers;
using Infrastructure.Providers.DefaultConfigProvider;
using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class InitializeDefaultConfigState : BaseState, IEnterableState
    {
        private readonly GameLoopStateMachine _gameLoopStateMachine;
        private readonly IDefaultConfigProvider _defaultConfigProvider;
        private readonly IConditionalLoggingService _conditionalLoggingService;

        public override string StateName => nameof(InitializeDefaultConfigState);

        [Inject]
        public InitializeDefaultConfigState(
            GameLoopStateMachine gameLoopStateMachine,
            IDefaultConfigProvider defaultConfigProvider,
            IConditionalLoggingService conditionalLoggingService
        )
        {
            _conditionalLoggingService = conditionalLoggingService;
            _defaultConfigProvider = defaultConfigProvider;
            _gameLoopStateMachine = gameLoopStateMachine;
        }

        private void ToNextState()
        {
            _gameLoopStateMachine.Enter<InitializeRemoteConfigState>();
        }

        private void OnLoadingSceneLoadedCallback()
        {
            ToNextState();
        }

        public void Enter()
        {
            Remote.InitializeByDefault(_defaultConfigProvider.CachedConfig, _conditionalLoggingService);

            _gameLoopStateMachine.Enter<LoadSceneState, Action>(OnLoadingSceneLoadedCallback);
        }
    }
}