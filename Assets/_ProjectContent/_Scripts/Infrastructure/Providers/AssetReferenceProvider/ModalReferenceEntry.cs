using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Providers.AssetReferenceProvider
{
    [Serializable]
    public class ModalReferenceEntry
    {
        [SerializeField] private string typeName;
        [SerializeField] private AssetReferenceGameObject assetReference;

        private Type cachedType;

        public Type Type
        {
            get
            {
                if (cachedType == null && !string.IsNullOrEmpty(typeName))
                    cachedType = Type.GetType(typeName);
                return cachedType;
            }
        }

        public AssetReferenceGameObject AssetReference => assetReference;

        public void SetType(Type type)
        {
            cachedType = type;
            typeName = type.AssemblyQualifiedName;
        }

        public void SetAssetReference(AssetReferenceGameObject reference)
        {
            assetReference = reference;
        }
    }
}