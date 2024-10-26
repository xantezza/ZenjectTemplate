using Cysharp.Threading.Tasks;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Services.Logging;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Modals
{
    public class ModalsFactory : MonoBehaviour
    {
        private AssetReferenceProvider _assetReferenceProvider;
        private ConditionalLoggingService _conditionalLoggingService;

        [Inject]
        public void Inject(AssetReferenceProvider assetReferenceProvider, ConditionalLoggingService conditionalLoggingService)
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