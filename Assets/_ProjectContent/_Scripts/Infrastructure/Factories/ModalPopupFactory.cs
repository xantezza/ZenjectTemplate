using Cysharp.Threading.Tasks;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Modals;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factories
{
    public class ModalPopupFactory : MonoBehaviour, IModalPopupFactory
    {
        private IAssetReferenceProvider _assetReferenceProvider;
        private IConditionalLoggingService _conditionalLoggingService;

        [Inject]
        public void Inject(IAssetReferenceProvider assetReferenceProvider, IConditionalLoggingService conditionalLoggingService)
        {
            _conditionalLoggingService = conditionalLoggingService;
            _assetReferenceProvider = assetReferenceProvider;
        }

        public async UniTask<T> Show<T>() where T : ModalPopup
        {
            var reference = _assetReferenceProvider.ModalsAssetReferences.TypeToReference<T>();
            if (reference == null)
            {
                _conditionalLoggingService.LogError($"In AssetReferenceProvider.ModalsAssetReferences not found reference to modal for type {typeof(T)}", LogTag.AssetReferenceProvider);
                return default;
            }
            
            var instantiated = await reference.InstantiateAsync(transform);
            var modalPopup = instantiated.GetComponent<T>();
            modalPopup.Show();
            return modalPopup;
        }
    }
}