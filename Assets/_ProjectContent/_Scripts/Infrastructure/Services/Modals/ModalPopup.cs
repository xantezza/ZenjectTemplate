using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Infrastructure.Factories.ModalPopup;
using LitMotion;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Extensions;

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
        protected IModalPopupFactory _modalPopupFactory;

        public void SetModalPopupFactory(IModalPopupFactory modalPopupFactory)
        {
            _modalPopupFactory = modalPopupFactory;
        }

        public async UniTask Show()
        {
            if (_modalSettings.ResizableRoot != null)
            {
                _modalSettings.ResizableRoot.localScale = _modalSettings.StartSize * Vector3.one;
                await _modalSettings.ResizableRoot.DOPopOutScale(
                    _modalSettings.MaxSize,
                    _modalSettings.TargetSize,
                    _modalSettings.ShowTime / 2,
                    _modalSettings.ShowTime / 2
                );
            }

            if (_modalSettings.CloseButton != null) _modalSettings.CloseButton.onClick.AddListener(Hide);
        }

        private async void Hide()
        {
            if (_modalSettings.CloseButton != null) _modalSettings.CloseButton.onClick.RemoveListener(Hide);

            if (_modalSettings.ResizableRoot != null)
            {
                MotionHandle motionHandle = await _modalSettings.ResizableRoot.DOPopOutScale(
                    _modalSettings.MaxSize,
                    _modalSettings.StartSize,
                    _modalSettings.HideTime / 2,
                    _modalSettings.HideTime / 2
                );
                await motionHandle;
            }

            OnInteract.Execute();
            ReturnToPool();
            await Task.CompletedTask;
        }

        protected abstract void ReturnToPool();
    }
}