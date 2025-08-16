namespace Infrastructure.Services.AudioService
{
    public interface IAudioService
    {
        void PlaySFX(SFXClip sfxClip, bool restartIfAlreadyExists = true);
        void PlayMusic(MusicClip musicClip, bool restartIfAlreadyExists = true);
        void StopMusic(MusicClip musicClip);

        void StopMusic();
    }
}