using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Infrastructure.Services.Logging;
using UnityEngine;

namespace Infrastructure.Services.Saving
{
    // used in production
    public class BinarySaveService : BaseSaveService
    {
        private static readonly byte[] Key = Convert.FromBase64String("c29tZUtleTExMjM0NTY3OA==");
        private static readonly byte[] IV = Convert.FromBase64String("c29tZUl2MTIzNDU2Nzg5MA==");

        protected virtual string _defaultFileName => "binaryDefaultSave";

        public BinarySaveService(IConditionalLoggingService loggingService) : base(loggingService)
        {
        }

        public override TSave Load<TSave>(IDataSaveable<TSave> dataSaveable)
        {
            if (_readyToSaveDictionary.TryGetValue(dataSaveable.SaveKey, out var value) && value is TSave save)
            {
               return save;
            }

            return null;
        }

        public override void LoadSaveFile(bool useDefaultFileName = true, string fileName = null)
        {
            if (useDefaultFileName) fileName = _defaultFileName;

            var path = $"{Application.persistentDataPath}/{fileName}.dat";

            if (!File.Exists(path))
            {
                _loggingService.Log("No game data to load", LogTag.SaveService);
                _hasLoaded = true;
                return;
            }

            _cachedSaveFileName = fileName;
            using var fileStream = File.OpenRead(path);
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using var cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            var binaryFormatter = new BinaryFormatter();

            _readyToSaveDictionary = (Dictionary<SaveKey, object>) binaryFormatter.Deserialize(cryptoStream);
            
            _hasLoaded = true;
            _loggingService.Log("Game data loaded!", LogTag.SaveService);
        }

        public override void StoreSaveFile(bool useDefaultFileName = true, string fileName = null)
        {
            if (useDefaultFileName || _cachedSaveFileName == null) fileName = _defaultFileName;
            else if (fileName == null && _cachedSaveFileName != null) fileName = _cachedSaveFileName;

            var path = $"{Application.persistentDataPath}/{fileName}.dat";
            var binaryFormatter = new BinaryFormatter();
            using var fileStream = File.Create(path);
            using var memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, _readyToSaveDictionary);
            var serializedData = memoryStream.ToArray();
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using var cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(serializedData, 0, serializedData.Length);

            _loggingService.Log($"Game data saved! At path: \n{path} \nContent is binary", LogTag.SaveService);
        }
    }
}