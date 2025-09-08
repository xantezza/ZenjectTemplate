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

        public void ForceShow()
        {
            _motionHandle?.TryCancel();
            SetProgress01(0);
            _canvasGroup.alpha = 1f;
            _canvas.enabled = true;
        }

        public void ForceHide()
        {
            _motionHandle?.TryCancel();
            SetProgress01(1);
            _canvasGroup.alpha = 0f;
            _canvas.enabled = false;
        }

        public async UniTask Show(float tweenDuration = 0.3f)
        {
            _motionHandle?.TryCancel();
            if (_canvas.enabled) return;
            SetProgress01(0);
            _canvas.enabled = true;
            _motionHandle = LMotion
                .Create(_canvasGroup.alpha, 1f, tweenDuration)
                .BindToAlpha(_canvasGroup);
            await ((MotionHandle) _motionHandle).ToUniTask();
        }

        public async UniTask Hide(float tweenDuration = 0.3f)
        {
            _motionHandle?.TryCancel();
            if (!_canvas.enabled) return;
            SetProgress01(1);
            _motionHandle = LMotion
                .Create(_canvasGroup.alpha, 0f, tweenDuration)
                .BindToAlpha(_canvasGroup);
            await ((MotionHandle) _motionHandle).ToUniTask();
            _canvas.enabled = false;
        }

        public void SetProgress01(float value)
        {
            _slider.value = value;
        }
    }
}