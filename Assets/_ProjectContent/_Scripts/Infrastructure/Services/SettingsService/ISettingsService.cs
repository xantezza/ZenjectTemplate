namespace Infrastructure.Services.SettingsService
{
    public interface ISettingsService
    {
        void MuteAudio();
        void UnMuteAudio();
        void SetMusicVolume(float value);
        void SetSFXVolume(float value);
    }
}