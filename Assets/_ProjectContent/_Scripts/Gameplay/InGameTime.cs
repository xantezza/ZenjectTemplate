﻿using System;
using Infrastructure.Services.Saving;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TimePassedLabel : MonoBehaviour, IDataSaveable<TimePassedLabel.Save>
    {
        [Serializable]
        public class Save
        {
            public float PassedTime;
        }

        public SaveKey SaveId => SaveKey.InGameTime;

        public Save SaveData { get; set; }

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
            _saveService.Process(this);
        }

        private void OnEnable()
        {
            _timeText.SetText(SaveData.PassedTime.ToString("N2"));
        }

        private void Update()
        {
            SaveData.PassedTime += Time.unscaledDeltaTime;

            _timeText.SetText(SaveData.PassedTime.ToString("N2"));
        }
    }
}