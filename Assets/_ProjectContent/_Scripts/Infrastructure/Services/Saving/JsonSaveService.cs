using System;
using System.Collections.Generic;
using System.IO;
using Infrastructure.Services.Logging;
using Newtonsoft.Json;
using UnityEngine;

namespace Infrastructure.Services.Saving
{
    // used in dev purposes
    public class JsonSaveService : BaseSaveService
    {
        protected virtual string _defaultFileName => "jsonDefaultSave";

        public JsonSaveService(IConditionalLoggingService loggingService) : base(loggingService)
        {
        }

        public override TSave Load<TSave>(IDataSaveable<TSave> dataSaveable)
        {
            if (_readyToSaveDictionary.TryGetValue(dataSaveable.SaveKey, out var value))
            {
                return JsonConvert.DeserializeObject<TSave>(JsonConvert.SerializeObject(value));
            }

            return null;
        }

        public override void LoadSaveFile(bool useDefaultFileName = true, string fileName = null)
        {
            if (useDefaultFileName) fileName = _defaultFileName;

            try
            {
                var path = $"{Application.persistentDataPath}/{fileName}.txt";

                if (!File.Exists(path))
                {
                    _loggingService.Log("No game data to load", LogTag.SaveService);
                    _hasLoaded = true;
                    return;
                }

                _cachedSaveFileName = fileName;

                var fileContent = File.ReadAllText(path);

                _readyToSaveDictionary = JsonConvert.DeserializeObject<Dictionary<SaveKey, object>>(fileContent);

                _hasLoaded = true;
                _loggingService.Log($"Game data loaded! \n{fileContent}", LogTag.SaveService);
            }
            catch (Exception e)
            {
                _loggingService.LogError($"Exception caught when loading save!\n{e}", LogTag.SaveService);
                _hasLoaded = true;
            }
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