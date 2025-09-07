using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Services.Pooling;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factories.ModalPopup
{
    public class ModalPopupFactory : MonoBehaviour, IModalPopupFactory
    {
        private IAssetReferenceProvider _assetReferenceProvider;

        private Dictionary<Type, object> _pools = new();

        [Inject]
        public void Inject(IAssetReferenceProvider assetReferenceProvider)
        {
            _assetReferenceProvider = assetReferenceProvider;
        }

        private AddressablesPool<T> GetPool<T>() where T : Services.Modals.ModalPopup
        {
            var type = typeof(T);
            if (_pools.TryGetValue(type, out var poolObj))
            {
                return (AddressablesPool<T>)poolObj;
            }

            var reference = _assetReferenceProvider.ModalsAssetReferences.TypeToReference<T>();
            var pool = new AddressablesPool<T>(reference, transform);
            _pools[type] = pool;
            return pool;
        }

        public async UniTask<T> Show<T>() where T : Services.Modals.ModalPopup
        {
            var pool = GetPool<T>();
            var modalPopup = await pool.GetAsync();
            modalPopup.SetModalPopupFactory(this);
            await modalPopup.Show();
            return modalPopup;
        }

        public async UniTask<T> Spawn<T>() where T : Services.Modals.ModalPopup
        {
            var pool = GetPool<T>();
            var modalPopup = await pool.GetAsync();
            modalPopup.SetModalPopupFactory(this);
            return modalPopup;
        }

        public void ReturnToPool<T>(T modalPopup) where T : Services.Modals.ModalPopup
        {
            var pool = GetPool<T>();
            pool.ReturnToPool(modalPopup);
        }
    }
}