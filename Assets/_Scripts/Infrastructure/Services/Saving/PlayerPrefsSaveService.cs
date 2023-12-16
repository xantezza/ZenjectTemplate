using System;
using Infrastructure.Services.Logging;
using UnityEngine;
using Utils.Extensions;
using Zenject;

namespace Infrastructure.Services.Saving
{
    public class PlayerPrefsSaveService : ISaveService
    {
        private const string NO_DATA_FOUND = "No data founded";

        private readonly IConditionalLoggingService _conditionalLoggingService;

        [Inject]
        public PlayerPrefsSaveService(IConditionalLoggingService conditionalLoggingService)
        {
            _conditionalLoggingService = conditionalLoggingService;
        }

        public void Dispose()
        {
        }

        public bool Save<TSave>(IDataSaveable<TSave> dataSaveable, bool overrideIfExist = true) where TSave : class
        {
            if (!overrideIfExist && PlayerPrefs.HasKey(dataSaveable.GetDataSaveId()))
            {
                _conditionalLoggingService.LogWarning($"Can't save data: data [key: {dataSaveable.GetDataSaveId()}] already exist!", LogTag.SaveService);
                return false;
            }

            return InternalSave(dataSaveable.GetDataSaveId(), dataSaveable.SaveData.ToJson());
        }

        public bool Load<TSave>(IDataSaveable<TSave> dataSaveable, bool createIfNotExist = true) where TSave : class
        {
            if (PlayerPrefs.HasKey(dataSaveable.GetDataSaveId())) return InternalLoad(dataSaveable);

            if (createIfNotExist)
            {
                _conditionalLoggingService.Log($"Load data [key: {dataSaveable.GetDataSaveId()}] is not founded! Create new data record", LogTag.SaveService);

                return InternalSave(dataSaveable.GetDataSaveId(), dataSaveable.ToJson()) && InternalLoad(dataSaveable);
            }

            _conditionalLoggingService.LogError($"Can't load data [key: {dataSaveable.GetDataSaveId()}]: key not founded!", LogTag.SaveService);
            return false;
        }

        public void DeleteAllData()
        {
            _conditionalLoggingService.LogWarning("PlayerPrefs DeleteAll was called", LogTag.SaveService);
            PlayerPrefs.DeleteAll();
        }

        public void StoreSaveData()
        {
            _conditionalLoggingService.Log("Successfully store save data", LogTag.SaveService);
            PlayerPrefs.Save();
        }

        private bool InternalSave(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            _conditionalLoggingService.Log($"Successfully save data [key: {key}][data: {value}]", LogTag.SaveService);
            return true;
        }

        private bool InternalLoad<TSave>(IDataSaveable<TSave> dataSaveable)
        {
            var data = PlayerPrefs.GetString(dataSaveable.GetDataSaveId(), NO_DATA_FOUND);

            if (data == NO_DATA_FOUND) data = dataSaveable.SaveData.ToJson();

            try
            {
                dataSaveable.SaveData = JsonUtility.FromJson<TSave>(data);

                _conditionalLoggingService.Log($"Successfully load data [key: {dataSaveable.GetDataSaveId()}][data: {data}]", LogTag.SaveService);

                return true;
            }
            catch (Exception ex)
            {
                _conditionalLoggingService.LogError($"[Exception] Can't load data [key: {dataSaveable.GetDataSaveId()}]: {ex.Message}\r\n{ex.StackTrace}", LogTag.SaveService);
                return false;
            }
        }
    }
}