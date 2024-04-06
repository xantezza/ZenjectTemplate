using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Providers.AssetReferenceProvider;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Services.Modals
{
    public class ModalsFactory
    {
        public enum ModalType
        {
            PrivacyPolicy,
            Test2
        }

        private readonly AssetReferenceProvider _assetReferenceProvider;

        public ModalsFactory(AssetReferenceProvider assetReferenceProvider)
        {
            _assetReferenceProvider = assetReferenceProvider;
        }

        public async UniTask<ModalPopup> Show(ModalType modalType)
        {
            var gameObject = await TypeToReference(modalType).InstantiateAsync();
            var modalPopup = gameObject.GetComponent<ModalPopup>();
            modalPopup.Show();
            return modalPopup;
        }

        public async UniTask<T> Show<T>(ModalType modalType) where T : ModalPopup
        {
            var gameObject = await TypeToReference(modalType).InstantiateAsync();
            var modalPopup = gameObject.GetComponent<T>();
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