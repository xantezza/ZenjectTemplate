﻿using System;
using Infrastructure.Services.Saving;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class InGameTimeText : MonoBehaviour, IDataSaveable<InGameTimeText.Save>
    {
        [Serializable]
        public class Save
        {
            public float PassedTime;
        }

        public Save SaveData { get; set; }

        public string GetDataSaveId()
        {
            return SaveKeys.InGameTime;
        }

        [SerializeField] private TextMeshProUGUI _timeText;

        private ISaveService _saveService;

        private void OnValidate()
        {
            _timeText = GetComponent<TextMeshProUGUI>();
        }

        [Inject]
        private void Inject(ISaveService saveService)
        {
            _saveService = saveService;
        }

        private void OnEnable()
        {
            _saveService.Load(this);
            _timeText.SetText(SaveData.PassedTime.ToString("N1"));
        }

        private void OnDisable()
        {
            _saveService.Save(this);
        }

        private void Update()
        {
            SaveData.PassedTime += Time.unscaledDeltaTime;
            _timeText.SetText(SaveData.PassedTime.ToString("N2"));
        }
    }
}