using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using LitMotion;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Infrastructure.Services.Modals
{
    [Serializable]
    public class ModalPopupSettings
    {
        public Button CloseButton;
        public RectTransform ResizableRoot;
        public float TargetSize = 1f;
        public float StartSize = 0.3f;
        public float MaxSize = 1.3f;
        public float ShowTime = 0.3f;
        public float HideTime = 0.3f;
    }

    public abstract class ModalPopup : MonoBehaviour
    {
        public readonly ReactiveCommand OnInteract = new();

        [SerializeField] private ModalPopupSettings _modalSettings;

        protected virtual void OnEnable()
        {
            if (_modalSettings.CloseButton != null) _modalSettings.CloseButton.onClick.AddListener(Hide);
        }

        protected virtual void OnDisable()
        {
            if (_modalSettings.CloseButton != null) _modalSettings.CloseButton.onClick.RemoveListener(Hide);
        }

        public async UniTask Show()
        {
            _modalSettings.ResizableRoot.localScale = _modalSettings.StartSize * Vector3.one;
            await _modalSettings.ResizableRoot.DOPopOutScale(
                _modalSettings.MaxSize,
                _modalSettings.TargetSize,
                _modalSettings.ShowTime / 2,
                _modalSettings.ShowTime / 2
            );
        }

        private async void Hide()
        {
            var motionHandle = await _modalSettings.ResizableRoot.DOPopOutScale(
                _modalSettings.MaxSize,
                _modalSettings.StartSize,
                _modalSettings.HideTime / 2,
                _modalSettings.HideTime / 2
            );

            OnInteract.Execute();
            await motionHandle;
            Destroy(gameObject);
            await Task.CompletedTask;
        }
    }
}