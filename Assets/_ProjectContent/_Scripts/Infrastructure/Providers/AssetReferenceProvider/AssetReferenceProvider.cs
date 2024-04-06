using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Providers.AssetReferenceProvider
{
    [Serializable]
    public class AssetReferenceProvider : MonoBehaviour
    {
        [field: SerializeField] public ModalsAssetReferences ModalsAssetReferences { get; private set; }
        
        [field: SerializeField] public AssetReferenceGameObject DebugRootAssetReference { get; private set; }
    }
}