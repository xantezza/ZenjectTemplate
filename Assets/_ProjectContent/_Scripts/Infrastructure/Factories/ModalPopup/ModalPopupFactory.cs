using Cysharp.Threading.Tasks;
using Infrastructure.Providers.AssetReferenceProvider;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factories.ModalPopup
{
    public class ModalPopupFactory : MonoBehaviour, IModalPopupFactory
    {
        private IAssetReferenceProvider _assetReferenceProvider;

        [Inject]
        public void Inject(IAssetReferenceProvider assetReferenceProvider)
        {
            _assetReferenceProvider = assetReferenceProvider;
        }

        public async UniTask<T> Show<T>() where T : Services.Modals.ModalPopup
        {
            var reference = _assetReferenceProvider.ModalsAssetReferences.TypeToReference<T>();
            var instantiated = await reference.InstantiateAsync(transform);
            var modalPopup = instantiated.GetComponent<T>();
            await modalPopup.Show();
            return modalPopup;
        }
        
        public async UniTask<T> Create<T>() where T : Services.Modals.ModalPopup
        {
            var reference = _assetReferenceProvider.ModalsAssetReferences.TypeToReference<T>();
            var instantiated = await reference.InstantiateAsync(transform);
            var modalPopup = instantiated.GetComponent<T>();
            return modalPopup;
        }
    }
}