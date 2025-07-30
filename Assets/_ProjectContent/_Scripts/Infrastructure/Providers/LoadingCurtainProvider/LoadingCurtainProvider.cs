using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Providers.LoadingCurtainProvider
{
    public class LoadingCurtainProvider : MonoBehaviour, ILoadingCurtainProvider
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Slider _slider;
        private TweenerCore<float, float, FloatOptions> _tweenerCore;

        private void Awake()
        {
            ForceHide();
        }

        public async UniTask Show(float tweenDuration = 0.3f)
        {
            if (_canvas.enabled) return;
            _canvas.enabled = true;
            _tweenerCore?.Kill();
            _tweenerCore = _canvasGroup.DOFade(1f, tweenDuration);
            await _tweenerCore.AsyncWaitForCompletion();
        }

        public void ForceShow()
        {
            if (_canvas.enabled) return;
            _tweenerCore?.Kill();
            _canvasGroup.alpha = 1f;
            _canvas.enabled = true;
        }

        public void SetProgress01(float value)
        {
            _slider.value = value;
        }

        public void Hide(float tweenDuration = 0.3f)
        {
            if (!_canvas.enabled) return;
            _tweenerCore?.Kill();
            _tweenerCore = _canvasGroup.DOFade(0f, tweenDuration);
            _tweenerCore.OnComplete(ForceHide);
        }

        public void ForceHide()
        {
            if (!_canvas.enabled) return;
            _tweenerCore?.Kill();
            _canvasGroup.alpha = 0f;
            _canvas.enabled = false;
        }
    }
}