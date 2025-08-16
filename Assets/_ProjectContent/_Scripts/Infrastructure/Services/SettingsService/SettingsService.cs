using System;
using Infrastructure.Services.Saving;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Infrastructure.Services.SettingsService
{
    public class SettingsService : MonoBehaviour, ISettingsService, IDataSaveable<SettingsService.Save>
    {
        [Serializable] public class Save
        {
            public float LastMusicValue = 0;
            public float LastSFXValue = 0;
        }

        [SerializeField] private AudioMixerGroup _sfxGroup;
        [SerializeField] private AudioMixerGroup _musicGroup;

        public SaveKey SaveKey => SaveKey.SettingsService;
        public Save SaveData { get; private set; }

        private ISaveService _saveService;

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

        public void MuteAudio()
        {
            _musicGroup.audioMixer.SetFloat("Volume", -100);
            _sfxGroup.audioMixer.SetFloat("Volume", -100);
        }

        public void UnMuteAudio()
        {
            SetMusicVolume(SaveData.LastMusicValue);
            SetSFXVolume(SaveData.LastSFXValue);
        }

        // from -100 to 20 (dB), 0 is default
        public void SetMusicVolume(float value)
        {
            SaveData.LastMusicValue = value;
            _musicGroup.audioMixer.SetFloat("Volume", value);
        }

        public void SetSFXVolume(float value)
        {
            SaveData.LastSFXValue = value;
            _sfxGroup.audioMixer.SetFloat("Volume", value);
        }
    }
}