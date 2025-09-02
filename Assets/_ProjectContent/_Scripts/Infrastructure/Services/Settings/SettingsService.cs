using System;
using Infrastructure.Services.Audio;
using Infrastructure.Services.Saving;
using Zenject;

namespace Infrastructure.Services.Settings
{
    public class SettingsService : ISettingsService, IDataSaveable<SettingsService.Save>, IInitializable
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
        private IAudioService _audioService;

        [Inject]
        private void Inject(ISaveService saveService, IAudioService audioService)
        {
            _audioService = audioService;
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
            _audioService.MusicMixerGroup.audioMixer.SetFloat("Volume", -100);
            _audioService.SfxMixerGroup.audioMixer.SetFloat("Volume", -100);
        }

        public void UnMuteAudio()
        {
            SetMusicVolume(SaveData.LastMusicValue);
            SetSFXVolume(SaveData.LastSFXValue);
        }

        public void SetMusicVolume(float value)
        {
            SaveData.LastMusicValue = value;
            _audioService.MusicMixerGroup.audioMixer.SetFloat("Volume", value);
        }

        public void SetSFXVolume(float value)
        {
            SaveData.LastSFXValue = value;
            _audioService.SfxMixerGroup.audioMixer.SetFloat("Volume", value);
        }
    }
}