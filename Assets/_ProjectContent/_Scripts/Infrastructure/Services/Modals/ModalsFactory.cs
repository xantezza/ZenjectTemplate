using Cysharp.Threading.Tasks;
using Infrastructure.Providers.AssetReferenceProvider;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Modals
{
    public class ModalsFactory : MonoBehaviour
    {
        private AssetReferenceProvider _assetReferenceProvider;

        [Inject]
        public void Inject(AssetReferenceProvider assetReferenceProvider)
        {
            _assetReferenceProvider = assetReferenceProvider;
        }

        public async UniTask<T> Show<T>() where T : ModalPopup
        {
            var instantiated = await _assetReferenceProvider.ModalsAssetReferences.TypeToReference<T>().InstantiateAsync(transform);
            var modalPopup = instantiated.GetComponent<T>();
            modalPopup.Show();
            return modalPopup;
        } 
    }
}