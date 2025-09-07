using Configs.RemoteConfig;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.LoadingCurtain;
using Infrastructure.Services.Log;
using JetBrains.Annotations;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Services.SceneLoading
{
    [UsedImplicitly]
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly ILoadingCurtainService _loadingCurtainService;
        private string _cachedSceneGUID;

        public SceneLoaderService(ILoadingCurtainService loadingCurtainService)
        {
            _loadingCurtainService = loadingCurtainService;
        }

        public async UniTask LoadScene(AssetReference nextSceneName, bool autoHideCurtain, bool allowReloadSameScene = false)
        {
            await LoadSceneByAddressables(nextSceneName, autoHideCurtain, allowReloadSameScene);
        }

        private async UniTask LoadSceneByAddressables(AssetReference nextScene, bool autoHideCurtain, bool allowReloadSameScene)
        {
            if (!allowReloadSameScene && _cachedSceneGUID == nextScene.AssetGUID)
            {
                Logger.Log("Scene tried to be loaded from itself, loading ignored", LogTag.SceneLoader);
                return;
            }

            await _loadingCurtainService.Show();
            var waitNextScene = Addressables.LoadSceneAsync(nextScene);
            
            while (!waitNextScene.IsDone)
            {
                _loadingCurtainService.SetProgress01(waitNextScene.PercentComplete);

                await UniTask.Yield();
            }
            
            Logger.Log($"Loaded scene: {waitNextScene.Result.Scene.name} \n{nextScene.AssetGUID}", LogTag.SceneLoader);

            await UniTask.WaitForSeconds(RemoteConfig.Infrastructure.FakeMinimalLoadTime);
            if (autoHideCurtain) await _loadingCurtainService.Hide();
        }
    }
}