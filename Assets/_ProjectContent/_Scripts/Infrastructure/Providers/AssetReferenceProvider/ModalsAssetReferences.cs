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

        public AssetReferenceGameObject TypeToReference<T>() where T : ModalPopup
        {
            if (typeof(T) == typeof(PrivacyPolicyModal)) return PrivacyPolicyModal;

            return null;
        }
    }
}