﻿using System;
using System.Collections.Generic;
using Infrastructure.Services.Logging;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Saving
{
    public abstract class BaseSaveService : ISaveService, IInitializable, IDisposable
    {
        protected Dictionary<SaveKey, object> _readyToSaveDictionary = new();

        protected readonly ConditionalLoggingService _loggingService;

        protected string _cachedSaveFileName;

        public void Process<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class, new()
        {
            Load(dataSaveable);
            AddToSave(dataSaveable);
        }

        public abstract void LoadSaveFile(bool useDefaultFileName = true, string fileName = null);

        public abstract void StoreSaveFile(bool useDefaultFileName = true, string fileName = null);

        [Inject]
        protected BaseSaveService(ConditionalLoggingService loggingService)
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

            StoreSaveFile(_cachedSaveFileName != null, _cachedSaveFileName);
        }

        protected abstract void Load<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class, new();

        private void AddToSave<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class
        {
            if (_readyToSaveDictionary.ContainsKey(dataSaveable.SaveId))
            {
                _readyToSaveDictionary[dataSaveable.SaveId] = dataSaveable.SaveData;
                return;
            }

            _readyToSaveDictionary.Add(dataSaveable.SaveId, dataSaveable.SaveData);
        }
    }
}