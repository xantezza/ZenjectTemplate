using System;
using Infrastructure.Services.Log;
using Infrastructure.Services.Modals;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Logger = Infrastructure.Services.Log.Logger;

namespace Infrastructure.Providers.AssetReferenceProvider
{
    [Serializable]
    public class ModalsAssetReferences : ScriptableObject
    {
        [field: SerializeField] public AssetReferenceGameObject PrivacyPolicyModal { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject ErrorModal { get; private set; }

        public AssetReferenceGameObject TypeToReference<T>() where T : ModalPopup
        {
            if (typeof(T) == typeof(PrivacyPolicyModal)) return PrivacyPolicyModal;
            if (typeof(T) == typeof(ErrorModal)) return ErrorModal;

            Logger.Error($"In AssetReferenceProvider.ModalsAssetReferences not found reference to modal for type {typeof(T)}", LogTag.AssetReferenceProvider);
            return null;
        }
    }
}