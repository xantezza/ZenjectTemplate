using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Saving;
using Infrastructure.Services.SceneLoading;
using UnityEngine;
using UnityEngine.Analytics;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class GameplayState : BaseGameLoopState, IEnterableState
    {
        private readonly ISaveService _saveService;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IAssetReferenceProvider _assetReferenceProvider;
        private readonly IAnalyticsService _analyticsService;

        [Inject]
        public GameplayState(
            GameLoopStateMachine gameLoopStateMachine,
            ISaveService saveService,
            ISceneLoaderService sceneLoaderService,
            IAssetReferenceProvider assetReferenceProvider,
            IAnalyticsService analyticsService
        ) : base(gameLoopStateMachine)
        {
            _analyticsService = analyticsService;
            _assetReferenceProvider = assetReferenceProvider;
            _sceneLoaderService = sceneLoaderService;
            _saveService = saveService;
        }

        public async UniTask Enter()
        {
            await _sceneLoaderService.LoadScene(_assetReferenceProvider.GamePlayScene);
        }

        public override UniTask Exit()
        {
            _analyticsService.SendEvent(AnalyticsNames.AverageFPS, Time.frameCount / Time.realtimeSinceStartup);
            _saveService.StoreSaveFile();
            return default;
        }
    }
}