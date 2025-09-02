using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils.Extensions;

namespace Infrastructure.Providers.AssetReferenceProvider
{
    [Serializable]
    [CreateAssetMenu]
    public class AssetReferenceProvider : ScriptableObject, IAssetReferenceProvider
    {
        [field: SerializeField] public ModalsAssetReferences ModalsAssetReferences { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject DebugRootAssetReference { get; private set; }
        [field: SerializeField] public AssetReference MenuScene { get; private set; }
        [field: SerializeField] public AssetReference GamePlayScene { get; private set; }

        public void ValidateReferences()
        {
#if UNITY_EDITOR
            NUnit.Framework.Assert.False(ReflectionUtils.GetClassPropertyInfo(this).Any(x => x == "[]"), "Missing reference!!");
            NUnit.Framework.Assert.False(ReflectionUtils.GetClassPropertyInfo(ModalsAssetReferences).Any(x => x == "[]"), "Missing reference!!");
#endif
        }
    }
}