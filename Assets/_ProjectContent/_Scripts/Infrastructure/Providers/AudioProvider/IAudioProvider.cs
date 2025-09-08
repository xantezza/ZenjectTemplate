using System.Collections.Generic;
using Infrastructure.Services.Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace Infrastructure.Providers.AudioProvider
{
    public interface IAudioProvider
    {
        AudioMixerGroup SFXGroup { get; }
        AudioMixerGroup MusicGroup { get; }
        IReadOnlyDictionary<SFXClip, AudioClip> SFXClips { get; }
        IReadOnlyDictionary<MusicClip, AudioClip> MusicClips { get; }
    }
}