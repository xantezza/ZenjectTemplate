﻿using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Providers.AssetReferenceProvider
{
    [Serializable]
    public class AssetReferenceProvider : MonoBehaviour
    {
        [field: SerializeField] public ModalsAssetReferences ModalsAssetReferences { get; private set; }

        [field: SerializeField] public AssetReferenceGameObject DebugRootAssetReference { get; private set; }
        [field: SerializeField] public AssetReference LoadingScene { get; private set; }
        [field: SerializeField] public AssetReference MenuScene { get; private set; }
        [field: SerializeField] public AssetReference GamePlayScene { get; private set; }
    }
}