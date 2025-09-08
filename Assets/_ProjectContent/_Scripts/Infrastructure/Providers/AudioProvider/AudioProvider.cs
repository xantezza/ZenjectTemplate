using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Infrastructure.Services.Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace Infrastructure.Providers.AudioProvider
{
    public class AudioProvider : ScriptableObject, IAudioProvider
    {
        [field: SerializeField] public AudioMixerGroup SFXGroup { get; private set; }
        [field: SerializeField] public AudioMixerGroup MusicGroup { get; private set; }
        
        [SerializeField] private SerializedDictionary<SFXClip, AudioClip> _SFXClips;
        [SerializeField] private SerializedDictionary<MusicClip, AudioClip> _musicClips;

        public IReadOnlyDictionary<SFXClip, AudioClip> SFXClips => _SFXClips;
        public IReadOnlyDictionary<MusicClip, AudioClip> MusicClips => _musicClips;
    }
}