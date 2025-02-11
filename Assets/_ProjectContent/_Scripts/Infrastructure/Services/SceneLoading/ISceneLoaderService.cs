using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Services.SceneLoading
{
    public interface ISceneLoaderService
    {
        UniTask LoadScene(AssetReference nextSceneName, Action onLoaded = null, bool allowReloadSameScene = false);
    }
}