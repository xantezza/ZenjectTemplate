using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Services.Analytics;
using Infrastructure.StateMachines.GameLoopStateMachine;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using Infrastructure.StateMachines.StateMachine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    public class InitializationFinalizerState : BaseInitializationState, IEnterableState
    {
        private readonly GameLoopStateMachineFactory _gameLoopStateMachineFactory;
        private readonly IAnalyticsService _analyticsService;
        private readonly AssetReferenceProvider _assetReferenceProvider;

        protected InitializationFinalizerState(
            InitializationStateMachine stateMachine,
            GameLoopStateMachineFactory gameLoopStateMachineFactory,
            IAnalyticsService analyticsService,
            AssetReferenceProvider assetReferenceProvider) : base(stateMachine)
        {
            _analyticsService = analyticsService;
            _assetReferenceProvider = assetReferenceProvider;
            _gameLoopStateMachineFactory = gameLoopStateMachineFactory;
        }

        public async UniTask Enter()
        {
            _analyticsService.SendEvent("load_finished");
            await _gameLoopStateMachineFactory.GetFrom(this).Enter<LoadingScreenState, AssetReference, Action>(_assetReferenceProvider.MenuScene, OnLoadingScreenLoaded);
        }

        private async void OnLoadingScreenLoaded()
        {
            await _gameLoopStateMachineFactory.GetFrom(this).Enter<MenuState>();
        }
    }
}