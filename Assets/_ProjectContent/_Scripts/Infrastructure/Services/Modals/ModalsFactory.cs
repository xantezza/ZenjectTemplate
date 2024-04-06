using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Providers.AssetReferenceProvider;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Infrastructure.Services.Modals
{
    public class ModalsFactory : MonoBehaviour
    {
        public enum ModalType
        {
            PrivacyPolicy,
            Test2
        }

        private AssetReferenceProvider _assetReferenceProvider;

        [Inject]
        public void Inject(AssetReferenceProvider assetReferenceProvider)
        {
            _assetReferenceProvider = assetReferenceProvider;
        }

        public async UniTask<ModalPopup> Show(ModalType modalType)
        {
            var instantiated = await TypeToReference(modalType).InstantiateAsync(transform);
            var modalPopup = instantiated.GetComponent<ModalPopup>();
            modalPopup.Show();
            return modalPopup;
        }

        public async UniTask<T> Show<T>(ModalType modalType) where T : ModalPopup
        {
            var instantiated = await TypeToReference(modalType).InstantiateAsync(transform);
            var modalPopup = instantiated.GetComponent<T>();
            modalPopup.Show();
            return modalPopup;
        }

        private AssetReferenceGameObject TypeToReference(ModalType modalType)
        {
            return modalType switch
            {
                ModalType.PrivacyPolicy => _assetReferenceProvider.ModalsAssetReferences.PrivacyPolicyModal,
                ModalType.Test2 => _assetReferenceProvider.ModalsAssetReferences.Test2Modal,
                _ => throw new ArgumentOutOfRangeException(nameof(modalType), modalType, null)
            };
        }
    }
}