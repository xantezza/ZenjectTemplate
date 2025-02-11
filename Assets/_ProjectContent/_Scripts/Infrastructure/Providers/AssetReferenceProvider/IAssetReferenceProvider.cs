using UnityEngine.AddressableAssets;

namespace Infrastructure.Providers.AssetReferenceProvider
{
    public interface IAssetReferenceProvider
    {
        ModalsAssetReferences ModalsAssetReferences { get; }
        AssetReferenceGameObject DebugRootAssetReference { get; }
        AssetReference MenuScene { get; }
        AssetReference GamePlayScene { get; }
    }
}