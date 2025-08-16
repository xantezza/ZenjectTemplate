using System;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Saving
{
    public class DataSaveableTemplate : MonoBehaviour, IDataSaveable<DataSaveableTemplate.Save>
    {
        private ISaveService _saveService;

        [Serializable]
        public class Save
        {
            public bool SomeField = false;
        }

        public SaveKey SaveKey => SaveKey.PrivacyPolicyState;
        public Save SaveData { get; private set; }

        //use of save service from [Inject] is prohibited!
        [Inject]
        private void Inject(ISaveService saveService)
        {
            _saveService = saveService;
        }
        
        private void Start()
        {
            SaveData = _saveService.Load(this) ?? new Save();
            _saveService.AddToSaveables(this);
        }
    }
}