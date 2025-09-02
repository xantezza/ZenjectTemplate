using System.Collections.Generic;
using Infrastructure.Providers.AudioProvider;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Infrastructure.Services.Audio
{
    public class AudioService : MonoBehaviour, IAudioService
    {
        private readonly Dictionary<SFXClip, AudioSource> _sfxSources = new();
        private readonly Dictionary<MusicClip, AudioSource> _musicSources = new();
        private readonly IAudioProvider _audioProvider;

        public AudioMixerGroup SfxMixerGroup => _audioProvider.SFXGroup;
        public AudioMixerGroup MusicMixerGroup => _audioProvider.MusicGroup;

        [Inject]
        public AudioService(IAudioProvider audioProvider)
        {
            _audioProvider = audioProvider;
        }
        public void PlaySFX(SFXClip sfxClip, float pitchDelta = 0, bool restartIfAlreadyExists = true)
        {
            if (_sfxSources.TryGetValue(sfxClip, out var existSource))
            {
                if (!restartIfAlreadyExists) return;
                existSource.Stop();
                existSource.Play();
                return;
            }

            var source = gameObject.AddComponent<AudioSource>();
            source.clip = _audioProvider.SFXClips[sfxClip];
            source.loop = false;
            source.outputAudioMixerGroup = _audioProvider.SFXGroup;
            source.pitch = pitchDelta != 0 ? Random.Range(1f - pitchDelta, 1f + pitchDelta) : 1f;
            source.Play();
            _sfxSources.Add(sfxClip, source);
        }

        public void PlayMusic(MusicClip musicClip, bool restartIfAlreadyExists = true)
        {
            if (_musicSources.TryGetValue(musicClip, out var existSource))
            {
                if (!restartIfAlreadyExists) return;
                existSource.Stop();
                existSource.Play();
                return;
            }

            var source = gameObject.AddComponent<AudioSource>();
            source.clip = _audioProvider.MusicClips[musicClip];
            source.loop = true;
            source.outputAudioMixerGroup = _audioProvider.MusicGroup;
            source.Play();
            _musicSources.Add(musicClip, source);
        }

        public void StopMusic(MusicClip musicClip)
        {
            if (_musicSources.TryGetValue(musicClip, out var existSource))
            {
                existSource.Stop();
            }
        }

        public void StopMusic()
        {
            foreach (var musicSourcesValue in _musicSources.Values)
            {
                musicSourcesValue.Stop();
            }
        }
    }
}