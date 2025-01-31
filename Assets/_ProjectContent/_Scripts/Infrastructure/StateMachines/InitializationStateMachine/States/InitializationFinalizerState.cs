using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Factories;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Services.Analytics;
using Infrastructure.StateMachines.GameLoopStateMachine;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using Infrastructure.StateMachines.StateMachine;
using JetBrains.Annotations;
using UnityEngine.AddressableAssets;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    [UsedImplicitly]
    public class InitializationFinalizerState : BaseInitializationState, IEnterableState
    {
        private readonly IGameLoopStateMachineFactory _gameLoopStateMachineFactory;
        private readonly IAnalyticsService _analyticsService;
        private readonly IAssetReferenceProvider _assetReferenceProvider;

        protected InitializationFinalizerState(
            InitializationStateMachine stateMachine,
            IGameLoopStateMachineFactory gameLoopStateMachineFactory,
            IAnalyticsService analyticsService,
            IAssetReferenceProvider assetReferenceProvider) : base(stateMachine)
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