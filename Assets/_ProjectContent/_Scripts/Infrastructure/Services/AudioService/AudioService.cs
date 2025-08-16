using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Audio;

namespace Infrastructure.Services.AudioService
{
    public class AudioService : MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioMixerGroup _sfxGroup;
        [SerializeField] private AudioMixerGroup _musicGroup;
        [SerializeField] private SerializedDictionary<SFXClip, AudioClip> _sfxClips;
        [SerializeField] private SerializedDictionary<MusicClip, AudioClip> _musicClips;
        private Dictionary<SFXClip, AudioSource> _sfxSources = new();
        private Dictionary<MusicClip, AudioSource> _musicSources = new();

        public AudioMixerGroup SfxMixerGroup => _sfxGroup;
        public AudioMixerGroup MusicMixerGroup => _musicGroup;

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
            source.clip = _sfxClips[sfxClip];
            source.loop = false;
            source.outputAudioMixerGroup = _sfxGroup;
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
            source.clip = _musicClips[musicClip];
            source.loop = true;
            source.outputAudioMixerGroup = _musicGroup;
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