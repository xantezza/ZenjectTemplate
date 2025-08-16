using UnityEngine.Audio;

namespace Infrastructure.Services.AudioService
{
    public interface IAudioService
    {
        void PlaySFX(SFXClip sfxClip, float pitchDelta = 0, bool restartIfAlreadyExists = true);
        void PlayMusic(MusicClip musicClip, bool restartIfAlreadyExists = true);
        void StopMusic(MusicClip musicClip);

        void StopMusic();
        
        AudioMixerGroup SfxMixerGroup { get; }
        AudioMixerGroup MusicMixerGroup { get; }
    }
}