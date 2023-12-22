using System;

namespace Infrastructure.Services.SceneLoading
{
    public interface ISceneLoaderService
    {
        void LoadScene(SceneNames nextSceneName, Action onLoaded = null, float minimalLoadTime = 0f, Action<float> onProgressUpdate = null);
        void LoadScene(int nextSceneBuildIndex, Action onLoaded = null, float minimalLoadTime = 0f, Action<float> onProgressUpdate = null);
    }
}