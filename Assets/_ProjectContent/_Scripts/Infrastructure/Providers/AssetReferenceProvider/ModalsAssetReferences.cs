using System;
using System.Collections.Generic;
using Infrastructure.Services.Log;
using Infrastructure.Services.Modals;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Logger = Infrastructure.Services.Log.Logger;

namespace Infrastructure.Providers.AssetReferenceProvider
{
    [CreateAssetMenu(fileName = "ModalsAssetReferences", menuName = "Infrastructure/ModalsAssetReferences")]
    public class ModalsAssetReferences : ScriptableObject
    {
        [SerializeField] private List<ModalReferenceEntry> modalReferences = new List<ModalReferenceEntry>();

        public AssetReferenceGameObject TypeToReference<T>() where T : ModalPopup
        {
            var type = typeof(T);
            foreach (var entry in modalReferences)
            {
                if (entry.Type == type)
                    return entry.AssetReference;
            }

            Logger.Error($"In AssetReferenceProvider.ModalsAssetReferences not found reference to modal for type {type}", LogTag.AssetReferenceProvider);
            return null;
        }

        // Пример метода для добавления или обновления записи (можно использовать в редакторе)
        public void AddOrUpdate(Type type, AssetReferenceGameObject reference)
        {
            var entry = modalReferences.Find(e => e.Type == type);
            if (entry != null)
            {
                entry.SetAssetReference(reference);
            }
            else
            {
                var newEntry = new ModalReferenceEntry();
                newEntry.SetType(type);
                newEntry.SetAssetReference(reference);
                modalReferences.Add(newEntry);
            }
        }
    }
}