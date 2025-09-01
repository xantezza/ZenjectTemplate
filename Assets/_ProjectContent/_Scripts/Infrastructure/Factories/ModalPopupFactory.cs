using Cysharp.Threading.Tasks;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Services.Log;
using Infrastructure.Services.Modals;
using UnityEngine;
using Zenject;
using Logger = Infrastructure.Services.Log.Logger;

namespace Infrastructure.Factories
{
    public class ModalPopupFactory : MonoBehaviour, IModalPopupFactory
    {
        private IAssetReferenceProvider _assetReferenceProvider;

        [Inject]
        public void Inject(IAssetReferenceProvider assetReferenceProvider)
        {
            _assetReferenceProvider = assetReferenceProvider;
        }

        public async UniTask<T> Show<T>() where T : ModalPopup
        {
            var reference = _assetReferenceProvider.ModalsAssetReferences.TypeToReference<T>();
            if (reference == null)
            {
                Logger.Error($"In AssetReferenceProvider.ModalsAssetReferences not found reference to modal for type {typeof(T)}", LogTag.AssetReferenceProvider);
                return default;
            }
            
            var instantiated = await reference.InstantiateAsync(transform);
            var modalPopup = instantiated.GetComponent<T>();
#pragma warning disable CS4014
            modalPopup.Show();
#pragma warning restore CS4014
            return modalPopup;
        }
    }
}