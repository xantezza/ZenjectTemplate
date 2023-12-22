using System;
using System.Collections;
using Infrastructure.Services.CoroutineRunner;
using Infrastructure.Services.Logging;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services.SceneLoading
{
    [UsedImplicitly]
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly ICoroutineRunnerService _coroutineRunner;
        private readonly IConditionalLoggingService _conditionalLoggingService;
        private int _cachedSceneIndex = 0;

        public SceneLoaderService(ICoroutineRunnerService coroutineRunner, IConditionalLoggingService conditionalLoggingService)
        {
            _conditionalLoggingService = conditionalLoggingService;
            _coroutineRunner = coroutineRunner;
        }

        public void LoadScene(SceneNames nextSceneName, Action onLoaded = null, float minimalLoadTime = 0f, Action<float> onProgressUpdate = null)
        {
            InternalLoadScene((int) nextSceneName, onLoaded, minimalLoadTime, onProgressUpdate);
        }

        public void LoadScene(int nextSceneBuildIndex, Action onLoaded = null, float minimalLoadTime = 0f, Action<float> onProgressUpdate = null)
        {
            InternalLoadScene(nextSceneBuildIndex, onLoaded, minimalLoadTime, onProgressUpdate);
        }

        private void InternalLoadScene(int nextSceneBuildIndex, Action onLoaded = null, float minimalLoadTime = 0f, Action<float> onProgressUpdate = null)
        {
            if (_cachedSceneIndex == nextSceneBuildIndex)
            {
                onLoaded?.Invoke();
                return;
            }

            _coroutineRunner.StartCoroutine(LoadSceneCoroutine(nextSceneBuildIndex, onLoaded, minimalLoadTime, onProgressUpdate));
        }

        private IEnumerator LoadSceneCoroutine(int nextScene, Action onLoaded, float minimalLoadTime, Action<float> onProgressUpdate)
        {
            _conditionalLoggingService.Log($"Loading scene: {SceneUtility.GetScenePathByBuildIndex(nextScene)} with wait {minimalLoadTime}", LogTag.SceneLoader);

            var timePassed = 0f;
            
            _cachedSceneIndex = nextScene;

            var waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            waitNextScene.allowSceneActivation = false;

            while (timePassed < minimalLoadTime)
            {
                timePassed += Time.unscaledDeltaTime;
                onProgressUpdate?.Invoke(waitNextScene.progress);
                yield return null;
            }

            waitNextScene.allowSceneActivation = true;

            while (!waitNextScene.isDone)
            {
                yield return null;
            }

            onLoaded?.Invoke();
        }
    }
}