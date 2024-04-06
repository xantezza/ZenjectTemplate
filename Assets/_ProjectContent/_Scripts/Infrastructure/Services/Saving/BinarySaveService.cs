﻿using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Infrastructure.Services.Logging;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Saving
{
    // used in production
    public class BinarySaveService : BaseSaveService
    {
        protected virtual string _defaultFileName => "binaryDefaultSave";

        public BinarySaveService(ConditionalLoggingService loggingService) : base(loggingService)
        {
        }

        protected override void Load<TSave>(IDataSaveable<TSave> dataSaveable)
        {
            if (_readyToSaveDictionary.TryGetValue(dataSaveable.SaveId, out var value) && value is TSave save)
            {
                dataSaveable.SaveData = save;
            }

            dataSaveable.SaveData ??= new TSave();
        }

        public override void LoadSaveFile(bool useDefaultFileName = true, string fileName = null)
        {
            if (useDefaultFileName) fileName = _defaultFileName;

            var path = $"{Application.persistentDataPath}/{fileName}.dat";

            if (!File.Exists(path))
            {
                _loggingService.Log("No game data to load", LogTag.SaveService);
                return;
            }

            _cachedSaveFileName = fileName;
            var binaryFormatter = new BinaryFormatter();
            var file = File.Open(path, FileMode.Open);
            _readyToSaveDictionary = (Dictionary<SaveKey, object>) binaryFormatter.Deserialize(file);

            file.Close();

            _loggingService.Log("Game data loaded!", LogTag.SaveService);
        }

        public override void StoreSaveFile(bool useDefaultFileName = true, string fileName = null)
        {
            if (useDefaultFileName || _cachedSaveFileName == null) fileName = _defaultFileName;
            else if (fileName == null && _cachedSaveFileName != null) fileName = _cachedSaveFileName;

            var path = $"{Application.persistentDataPath}/{fileName}.dat";
            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Create(path);
            binaryFormatter.Serialize(fileStream, _readyToSaveDictionary);
            fileStream.Close();
            _loggingService.Log($"Game data saved! At path: \n{path} \nContent is binary", LogTag.SaveService);
        }
    }
}