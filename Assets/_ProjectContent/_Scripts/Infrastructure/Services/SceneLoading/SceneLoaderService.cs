using System;
using Configs.RemoteConfig;
using Cysharp.Threading.Tasks;
using Infrastructure.Providers.LoadingCurtainProvider;
using Infrastructure.Services.Logging;
using JetBrains.Annotations;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Infrastructure.Services.SceneLoading
{
    [UsedImplicitly]
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly IConditionalLoggingService _conditionalLoggingService;
        private readonly ILoadingCurtainProvider _loadingCurtainProvider;
        private string _cachedSceneGUID;

        public SceneLoaderService(IConditionalLoggingService conditionalLoggingService, ILoadingCurtainProvider loadingCurtainProvider)
        {
            _loadingCurtainProvider = loadingCurtainProvider;
            _conditionalLoggingService = conditionalLoggingService;
        }

        public async UniTask LoadScene(AssetReference nextSceneName, Action onLoaded = null, bool allowReloadSameScene = false)
        {
            await LoadSceneByAddressables(nextSceneName, onLoaded, allowReloadSameScene);
        }

        private async UniTask LoadSceneByAddressables(AssetReference nextScene, Action onLoaded, bool allowReloadSameScene)
        {
            if (!allowReloadSameScene && _cachedSceneGUID == nextScene.AssetGUID)
            {
                _conditionalLoggingService.Log("Scene tried to be loaded from itself, loading ignored", LogTag.SceneLoader);
                onLoaded?.Invoke();
                return;
            }

            await _loadingCurtainProvider.Show();
            AsyncOperationHandle<SceneInstance> waitNextScene = Addressables.LoadSceneAsync(nextScene);
            
            while (!waitNextScene.IsDone)
            {
                _loadingCurtainProvider.SetProgress01(waitNextScene.PercentComplete);

                await UniTask.Yield();
            }
            
            _conditionalLoggingService.Log($"Loaded scene: {waitNextScene.Result.Scene.name} \n{nextScene.AssetGUID}", LogTag.SceneLoader);

            onLoaded?.Invoke();
            await UniTask.WaitForSeconds(RemoteConfig.Infrastructure.FakeMinimalLoadTime);
            _loadingCurtainProvider.Hide();
        }
    }
}