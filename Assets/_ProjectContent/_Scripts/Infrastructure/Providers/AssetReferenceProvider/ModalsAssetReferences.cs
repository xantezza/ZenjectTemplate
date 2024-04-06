using System;
using Infrastructure.Services.Modals;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Providers.AssetReferenceProvider
{
    [Serializable]
    public class ModalsAssetReferences
    {
        [SerializeField] private AssetReferenceGameObject _privacyPolicyModal;


        public AssetReferenceGameObject TypeToReference<T>() where T : ModalPopup
        {
            if (typeof(T) == typeof(PrivacyPolicyModal)) return _privacyPolicyModal;

            return null;
        }
    }
}