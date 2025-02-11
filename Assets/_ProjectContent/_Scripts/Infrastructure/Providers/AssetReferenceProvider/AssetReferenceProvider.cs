using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils.Extensions;

namespace Infrastructure.Providers.AssetReferenceProvider
{
    [Serializable]
    public class AssetReferenceProvider : IAssetReferenceProvider
    {
        [field: SerializeField] public ModalsAssetReferences ModalsAssetReferences { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject DebugRootAssetReference { get; private set; }
        [field: SerializeField] public AssetReference MenuScene { get; private set; }
        [field: SerializeField] public AssetReference GamePlayScene { get; private set; }

        public void ValidateReferences()
        {
#if UNITY_EDITOR
          
            Assert.False(ReflectionUtils.GetClassPropertyInfo(this).Any(x => x == "[]"), "Missing reference!!");
           Assert.False(ReflectionUtils.GetClassPropertyInfo(ModalsAssetReferences).Any(x => x == "[]"), "Missing reference!!");  
#endif
        }
    }
}