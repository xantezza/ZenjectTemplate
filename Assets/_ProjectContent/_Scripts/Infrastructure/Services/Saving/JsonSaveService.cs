﻿using System.Collections.Generic;
using System.IO;
using Infrastructure.Services.Logging;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Saving
{
    // used in dev purposes
    public class JsonSaveService : BaseSaveService
    {
        protected virtual string _defaultFileName => "jsonDefaultSave";

        public JsonSaveService(ConditionalLoggingService loggingService) : base(loggingService)
        {
        }

        protected override void Load<TSave>(IDataSaveable<TSave> dataSaveable)
        {
            if (_readyToSaveDictionary.TryGetValue(dataSaveable.SaveId, out var value))
            {
                dataSaveable.SaveData = JsonConvert.DeserializeObject<TSave>(JsonConvert.SerializeObject(value));
            }

            dataSaveable.SaveData ??= new TSave();
        }
        
        public override void LoadSaveFile(bool useDefaultFileName = true, string fileName = null)
        {
            if (useDefaultFileName) fileName = _defaultFileName;

            var path = $"{Application.persistentDataPath}/{fileName}.txt";

            if (!File.Exists(path))
            {
                _loggingService.Log("No game data to load", LogTag.SaveService);
                return;
            }

            _cachedSaveFileName = fileName;

            var file = File.ReadAllText(path);

            _readyToSaveDictionary = JsonConvert.DeserializeObject<Dictionary<SaveKey, object>>(file);

            _loggingService.Log("Game data loaded!", LogTag.SaveService);
        }

        public override void StoreSaveFile(bool useDefaultFileName = true, string fileName = null)
        {
            if (useDefaultFileName || _cachedSaveFileName == null) fileName = _defaultFileName;
            else if (fileName == null && _cachedSaveFileName != null) fileName = _cachedSaveFileName;

            var path = $"{Application.persistentDataPath}/{fileName}.txt";
            var serializedObject = JsonConvert.SerializeObject(_readyToSaveDictionary, Formatting.Indented);
            File.WriteAllText(path, serializedObject);
            _loggingService.Log($"Game data saved! At path: \n{path} \nContent: \n{serializedObject}", LogTag.SaveService);
        }
    }
}