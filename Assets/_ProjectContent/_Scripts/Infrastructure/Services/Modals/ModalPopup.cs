using System;
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
        public float StartSize;
        public float MaxSize;
        public float ShowTime;
        public float HideTime;
    }

    public abstract class ModalPopup : MonoBehaviour
    {
        public ReactiveCommand OnInteract = new();

        [SerializeField] private ModalPopupSettings _modalSettings;

        protected virtual void OnEnable()
        {
            _modalSettings.CloseButton.onClick.AddListener(Hide);
        }

        protected virtual void OnDisable()
        {
            _modalSettings.CloseButton.onClick.RemoveListener(Hide);
        }

        public void Show()
        {
            _modalSettings.ResizableRoot.DOPopOutScale(
                Vector3.one * _modalSettings.MaxSize,
                Vector3.one,
                _modalSettings.ShowTime / 2,
                _modalSettings.ShowTime / 2
            );
        }

        private void Hide()
        {
            _modalSettings.ResizableRoot.DOPopOutScale(
                Vector3.one * _modalSettings.MaxSize,
                Vector3.one * _modalSettings.StartSize,
                _modalSettings.HideTime / 2,
                _modalSettings.HideTime / 2,
                () => Destroy(gameObject)
            );

            OnInteract.Execute();
        }
    }
}