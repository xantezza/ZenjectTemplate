using System;
using System.Collections.Generic;
using Infrastructure.Services.Logging;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Saving
{
    public abstract class BaseSaveService : ISaveService, IInitializable, IDisposable
    {
        protected Dictionary<SaveKey, object> _readyToSaveDictionary = new();

        protected readonly IConditionalLoggingService _loggingService;

        protected string _cachedSaveFileName;
        protected bool _hasLoaded;
        
        public abstract TSave Load<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class;

        public void AddToSaveables<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class
        {
            if (_readyToSaveDictionary.ContainsKey(dataSaveable.SaveKey))
            {
                _readyToSaveDictionary[dataSaveable.SaveKey] = dataSaveable.SaveData;
                return;
            }

            _readyToSaveDictionary.Add(dataSaveable.SaveKey, dataSaveable.SaveData);
        }

        public abstract void LoadSaveFile(bool useDefaultFileName = true, string fileName = null);

        public abstract void StoreSaveFile(bool useDefaultFileName = true, string fileName = null);

        [Inject]
        protected BaseSaveService(IConditionalLoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void Initialize()
        {
            Application.focusChanged += OnApplicationFocus;
        }

        public void Dispose()
        {
            Application.focusChanged -= OnApplicationFocus;
        }

        private void OnApplicationFocus(bool focusStatus)
        {
            if (focusStatus) return;
            if (!Application.isPlaying) return;
            if (_readyToSaveDictionary.Count == 0) return;
            if (!_hasLoaded) return;

            StoreSaveFile(_cachedSaveFileName != null, _cachedSaveFileName);
        }
    }
}