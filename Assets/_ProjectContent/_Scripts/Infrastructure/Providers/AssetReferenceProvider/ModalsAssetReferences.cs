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
        [field: SerializeField] public AssetReferenceGameObject ErrorModal { get; private set; }

        public AssetReferenceGameObject TypeToReference<T>() where T : ModalPopup
        {
            if (typeof(T) == typeof(PrivacyPolicyModal)) return PrivacyPolicyModal;
            if (typeof(T) == typeof(ErrorModal)) return ErrorModal;

            return null;
        }
    }
}