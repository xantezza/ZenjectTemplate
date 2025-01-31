using UnityEngine.AddressableAssets;

namespace Infrastructure.Providers.AssetReferenceProvider
{
    public interface IAssetReferenceProvider
    {
        ModalsAssetReferences ModalsAssetReferences { get; }
        AssetReferenceGameObject DebugRootAssetReference { get; }
        AssetReferenceGameObject InventoryViewAssetReference { get; }
        AssetReference LoadingScene { get; }
        AssetReference MenuScene { get; }
        AssetReference GamePlayScene { get; }
        void ValidateReferences();
    }
}