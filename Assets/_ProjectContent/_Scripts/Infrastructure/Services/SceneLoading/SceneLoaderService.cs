using System;
using System.Collections;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Logging;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services.SceneLoading
{
    [UsedImplicitly]
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly ICoroutineRunnerService _coroutineRunner;
        private readonly ConditionalLoggingService _conditionalLoggingService;
        private string _cachedSceneGUID;

        public SceneLoaderService(ICoroutineRunnerService coroutineRunner, ConditionalLoggingService conditionalLoggingService)
        {
            _conditionalLoggingService = conditionalLoggingService;
            _coroutineRunner = coroutineRunner;
        }

        public void LoadScene(AssetReference nextSceneName, bool allowReloadSameScene = false, Action onLoaded = null, float minimalLoadTime = 0f, Action<float> onProgressUpdate = null)
        {
            _coroutineRunner.StartCoroutine(LoadSceneByAddressablesCoroutine(nextSceneName, allowReloadSameScene, onLoaded, minimalLoadTime, onProgressUpdate));
        }


        private IEnumerator LoadSceneByAddressablesCoroutine(AssetReference nextScene, bool allowReloadSameScene, Action onLoaded, float minimalLoadTime, Action<float> onProgressUpdate)
        {
            _conditionalLoggingService.Log($"Loading scene: {nextScene} with wait {minimalLoadTime}", LogTag.SceneLoader);

            if (!allowReloadSameScene && _cachedSceneGUID == nextScene.AssetGUID)
            {
                _conditionalLoggingService.Log("Scene tried to be loaded from itself, loading ignored", LogTag.SceneLoader);
                onLoaded?.Invoke();
                yield break;
            }

            var timePassed = 0f;

            _cachedSceneGUID = nextScene.AssetGUID;

            var waitNextScene = Addressables.LoadSceneAsync(nextScene, LoadSceneMode.Single, false);

            while (timePassed < minimalLoadTime)
            {
                timePassed += Time.unscaledDeltaTime;
                onProgressUpdate?.Invoke(waitNextScene.PercentComplete);
                yield return null;
            }

            while (!waitNextScene.IsDone)
            {
                yield return null;
            }

            waitNextScene.Result.ActivateAsync();
            _conditionalLoggingService.Log($"Loaded scene: {waitNextScene.Result.Scene.name} \n{nextScene.AssetGUID}", LogTag.SceneLoader);
            onLoaded?.Invoke();
        }
    }
}