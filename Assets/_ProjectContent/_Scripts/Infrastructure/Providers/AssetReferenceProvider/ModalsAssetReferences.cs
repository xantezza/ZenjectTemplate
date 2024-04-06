using System;
using Infrastructure.Services.Modals;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Providers.AssetReferenceProvider
{
    [Serializable]
    public class ModalsAssetReferences
    {
        [field: SerializeField] public AssetReferenceGameObject PrivacyPolicyModal { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject Test2Modal { get; private set; }
    }
}