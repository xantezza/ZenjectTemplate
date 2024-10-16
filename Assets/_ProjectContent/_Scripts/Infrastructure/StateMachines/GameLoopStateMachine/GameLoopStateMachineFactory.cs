﻿using Infrastructure.Services.Logging;
using JetBrains.Annotations;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine
{
    [UsedImplicitly]
    public class GameLoopStateMachineFactory
    {
        private GameLoopStateMachine _stateMachine;
        private readonly ConditionalLoggingService _loggingService;
        private readonly IInstantiator _instantiator;

        [Inject]
        public GameLoopStateMachineFactory(IInstantiator instantiator, ConditionalLoggingService loggingService)
        {
            _instantiator = instantiator;
            _loggingService = loggingService;
        }

        public GameLoopStateMachine GetFrom(object summoner)
        {
            _stateMachine ??= _instantiator.Instantiate<GameLoopStateMachine>();
            _loggingService.Log($"Access to the {this} is obtained from {summoner}", LogTag.GameLoopStateMachine);
            return _stateMachine;
        }
    }
}