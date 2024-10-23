using System;
using Infrastructure.Services.OnGUIService;
using Infrastructure.Services.Saving;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TimePassedLabel : MonoBehaviour, IDataSaveable<TimePassedLabel.Save>, IDevOnGUIElement
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
        private IOnGUIService _onGUIService;

        private void OnValidate()
        {
            _timeText = GetComponent<TextMeshProUGUI>();
        }

        [Inject]
        private void Inject(ISaveService saveService, IOnGUIService onGUIService)
        {
            _onGUIService = onGUIService;
            _onGUIService.AddDevOnGUIElement(this);
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

        public void DrawDevGUI()
        {
            GUILayout.Label($"{SaveData.PassedTime}");
        }
    }
}