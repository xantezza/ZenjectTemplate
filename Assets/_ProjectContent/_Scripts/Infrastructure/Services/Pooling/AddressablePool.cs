using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Infrastructure.Services.Pooling
{
    public class AddressablesPool<T> where T : Component
    {
        private readonly AssetReference _reference;
        private readonly Transform _parent;
        private readonly Transform _pooledObjectsParent;
        private readonly Stack<T> _pool = new();

        public AddressablesPool(AssetReference reference, Transform parent = null)
        {
            _reference = reference;
            _parent = parent;
            
            _pooledObjectsParent = new GameObject("Pool Root").transform;
            _pooledObjectsParent.parent = _parent;
        }
                
        public async Task<T> GetAsync()
        {
            if (_pool.Count > 0)
            {
                var item = _pool.Pop();
                item.gameObject.SetActive(true);
                item.gameObject.transform.SetParent(_parent, false);
                return item;
            }

            var handle = _reference.InstantiateAsync(_parent);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var instance = handle.Result;
                var component = instance.GetComponent<T>();
                if (component != null) return component;
                Debug.LogError($"Object dont have {typeof(T)}");
                _reference.ReleaseInstance(instance);
                return null;
            }

            Debug.LogError("Cant get object from Addressables");
            return null;
        }

        public void ReturnToPool(T item)
        {
            if (item == null)
                return;

            item.gameObject.SetActive(false);
            item.gameObject.transform.SetParent(_pooledObjectsParent, false);
            _pool.Push(item);
        }

        public void Clear()
        {
            while (_pool.Count > 0)
            {
                var item = _pool.Pop();
                if (item != null)
                {
                    Object.Destroy(item.gameObject);
                }
            }
        }
    }
}