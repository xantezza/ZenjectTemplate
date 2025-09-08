using System;
using Infrastructure.Providers.AudioProvider;
using Infrastructure.Services.Saving;
using Zenject;

namespace Infrastructure.Services.Settings
{
    public class SettingsService : ISettingsService, IInitializable, IDataSaveable<SettingsService.Save>
    {
        [Serializable]
        public class Save
        {
            public float LastMusicValue = 0;
            public float LastSFXValue = 0;
        }

        public SaveKey SaveKey => SaveKey.SettingsService;
        public Save SaveData { get; private set; }

        private ISaveService _saveService;
        private IAudioProvider _audioProvider;

        [Inject]
        private void Inject(ISaveService saveService, IAudioProvider audioProvider)
        {
            _audioProvider = audioProvider;
            _saveService = saveService;
        }

        public void Initialize()
        {
            SaveData = _saveService.Load(this) ?? new Save();
            _saveService.AddToSaveables(this);
            SetMusicVolume(SaveData.LastMusicValue);
            SetSFXVolume(SaveData.LastSFXValue);
        }

        public void MuteAudio()
        {
            _audioProvider.MusicGroup.audioMixer.SetFloat("Volume", -100);
            _audioProvider.SFXGroup.audioMixer.SetFloat("Volume", -100);
        }

        public void UnMuteAudio()
        {
            SetMusicVolume(SaveData.LastMusicValue);
            SetSFXVolume(SaveData.LastSFXValue);
        }

        public void SetMusicVolume(float value)
        {
            SaveData.LastMusicValue = value;
            _audioProvider.MusicGroup.audioMixer.SetFloat("Volume", value);
        }

        public void SetSFXVolume(float value)
        {
            SaveData.LastSFXValue = value;
            _audioProvider.SFXGroup.audioMixer.SetFloat("Volume", value);
        }
    }
}