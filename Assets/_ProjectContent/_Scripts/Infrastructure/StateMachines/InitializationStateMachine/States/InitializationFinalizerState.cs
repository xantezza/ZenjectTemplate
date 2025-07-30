using Cysharp.Threading.Tasks;
using Infrastructure.Factories;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines.GameLoopStateMachine.States;
using Infrastructure.StateMachines.StateMachine;
using JetBrains.Annotations;

namespace Infrastructure.StateMachines.InitializationStateMachine.States
{
    [UsedImplicitly]
    public class InitializationFinalizerState : BaseInitializationState, IEnterableState
    {
        private readonly IGameLoopStateMachineFactory _gameLoopStateMachineFactory;
        private readonly IAnalyticsService _analyticsService;
        private readonly IAssetReferenceProvider _assetReferenceProvider;
        private readonly ISceneLoaderService _sceneLoaderService;

        protected InitializationFinalizerState(
            InitializationStateMachine stateMachine,
            ISceneLoaderService sceneLoaderService,
            IGameLoopStateMachineFactory gameLoopStateMachineFactory,
            IAnalyticsService analyticsService,
            IAssetReferenceProvider assetReferenceProvider) : base(stateMachine)
        {
            _sceneLoaderService = sceneLoaderService;
            _analyticsService = analyticsService;
            _assetReferenceProvider = assetReferenceProvider;
            _gameLoopStateMachineFactory = gameLoopStateMachineFactory;
        }

        public async UniTask Enter()
        {
            _analyticsService.SendEvent("load_finished");
            await _sceneLoaderService.LoadScene(_assetReferenceProvider.MenuScene, OnSceneLoaded);
        }

        private async void OnSceneLoaded()
        {
            await _gameLoopStateMachineFactory.GetFrom(this).Enter<MenuState>();
        }
    }
}