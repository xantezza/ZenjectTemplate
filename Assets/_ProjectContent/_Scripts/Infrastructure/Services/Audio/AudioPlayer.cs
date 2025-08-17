using TriInspector;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private bool _playOnEnable;
        [SerializeField] private bool _stopOnDisable;
        [SerializeField] private bool _sfx;
        [SerializeField] [ShowIf("_sfx")] private SFXClip _sfxClip;
        [SerializeField] [HideIf("_sfx")] private MusicClip _musicClip;
        private IAudioService _audioService;

        [Inject] private void Inject(IAudioService audioService)
        {
            _audioService = audioService;
        }
        
        private void OnEnable()
        {
            if (_playOnEnable)
            {
                if (_sfx) _audioService.PlaySFX(_sfxClip);
                else _audioService.PlayMusic(_musicClip);
            }
        }

        public void Play()
        {
            if (_sfx) _audioService.PlaySFX(_sfxClip);
            else _audioService.PlayMusic(_musicClip);
        }

        public void PlaySFX(SFXClip clip)
        {
            _audioService.PlaySFX(clip);
        }
        
        public void PlayMusic(MusicClip clip)
        {
            _audioService.PlayMusic(clip);
        }

        private void OnDisable()
        {
            if (_stopOnDisable && !_sfx) _audioService.StopMusic(_musicClip);
        }
    }
}