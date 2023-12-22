using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Infrastructure.Services.Logging;
using Infrastructure.Services.SceneLoading;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.Services.Saving
{
    public abstract class BaseSaveService : ISaveService
    {
        protected Dictionary<string, object> _readyToSaveDictionary = new();

        protected readonly IConditionalLoggingService _loggingService;

        protected string _cachedSaveFileName;
        protected abstract string _defaultFileName { get; }

        public abstract void LoadSaveFile(bool useDefaultFileName = true, string fileName = null);

        public abstract void StoreSaveFile(bool useDefaultFileName = true, string fileName = null);

        [Inject]
        protected BaseSaveService(IConditionalLoggingService loggingService)
        {
            _loggingService = loggingService;
#if !UNITY_EDITOR
            SubscribeAutoSave();
#endif
        }

        [UsedImplicitly]
        private void SubscribeAutoSave()
        {
            Application.quitting += OnApplicationQuitting;

            void OnApplicationQuitting()
            {
                if (SceneManager.GetActiveScene().buildIndex != (int) SceneNames.Gameplay) return;
                Application.quitting -= OnApplicationQuitting;
                var hasCachedFileName = _cachedSaveFileName != null;
                StoreSaveFile(hasCachedFileName, _cachedSaveFileName);
            }
        }

        public void Load<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class
        {
            if (_readyToSaveDictionary.TryGetValue(dataSaveable.SaveId(), out var value) && value is TSave save)
            {
                dataSaveable.SaveData = save;
            }
            else
            {
                dataSaveable.SaveData = dataSaveable.Default;
            }
        }

        public void AddToSave<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class
        {
            if (_readyToSaveDictionary.ContainsKey(dataSaveable.SaveId()))
            {
                _readyToSaveDictionary[dataSaveable.SaveId()] = dataSaveable.SaveData;
                return;
            }

            _readyToSaveDictionary.Add(dataSaveable.SaveId(), dataSaveable.SaveData);
        }
    }
}