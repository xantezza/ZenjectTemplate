using System;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Services.SceneLoading
{
    public interface ISceneLoaderService
    {
        void LoadScene(AssetReference nextSceneName, bool allowReloadSameScene = false, Action onLoaded = null, float minimalLoadTime = 0f, Action<float> onProgressUpdate = null);
    }
}