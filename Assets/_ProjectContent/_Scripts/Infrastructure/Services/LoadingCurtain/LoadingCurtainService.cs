using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Services.LoadingCurtain
{
    public class LoadingCurtainService : MonoBehaviour, ILoadingCurtainService
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Slider _slider;
        private MotionHandle? _motionHandle;

        private void Awake()
        {
            ForceHide();
        }

        public async UniTask Show(float tweenDuration = 0.3f)
        {
            if (_canvas.enabled) return;
            _canvas.enabled = true;
            _motionHandle?.TryCancel();
            _motionHandle = LMotion
                .Create(_canvasGroup.alpha, 1f, tweenDuration)
                .BindToAlpha(_canvasGroup);
            await ((MotionHandle)_motionHandle).ToUniTask();
        }

        public void ForceShow()
        {
            if (_canvas.enabled) return;
            _motionHandle?.TryCancel();
            _canvasGroup.alpha = 1f;
            _canvas.enabled = true;
        }

        public void SetProgress01(float value)
        {
            _slider.value = value;
        }

        public async void Hide(float tweenDuration = 0.3f)
        {
            if (!_canvas.enabled) return;
            
            _motionHandle?.TryCancel();
            _motionHandle = LMotion
                .Create(_canvasGroup.alpha, 0f, tweenDuration)
                .BindToAlpha(_canvasGroup);
            await ((MotionHandle)_motionHandle).ToUniTask();
            _canvas.enabled = false;
        }

        public void ForceHide()
        {
            if (!_canvas.enabled) return;
            _motionHandle?.TryCancel();
            _canvasGroup.alpha = 0f;
            _canvas.enabled = false;
        }
    }
}