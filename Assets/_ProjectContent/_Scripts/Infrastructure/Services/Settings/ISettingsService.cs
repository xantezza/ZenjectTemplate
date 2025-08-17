namespace Infrastructure.Services.Settings
{
    public interface ISettingsService
    {
        void MuteAudio();
        void UnMuteAudio();
        void SetMusicVolume(float value);
        void SetSFXVolume(float value);
    }
}