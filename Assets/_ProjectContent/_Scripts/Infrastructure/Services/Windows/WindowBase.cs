using System;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Windows
{
    public interface IWindowBase
    {
        public Action<Type> OnBeforeShow { get; set; }
        public Action<Type> OnBeforeHide { get; set; }
        public Action<Type> OnAfterShow { get; set; }
        public Action<Type> OnAfterHide { get; set; }
        public UniTask Show();
        public UniTask Hide();
        public void Toggle();
        public bool IsShowing { get; }
        public bool InQueue { get; set; }
        public IWindowService WindowService { get; }
        public int QueuePriority { get; }
    }

    [DeclareBoxGroup("Buttons")]
    public abstract class WindowBase<T> : MonoBehaviour, IWindowBase where T : IWindowBase
    {
        public Action<Type> OnBeforeShow { get; set; }
        public Action<Type> OnBeforeHide { get; set; }
        public Action<Type> OnAfterShow { get; set; }
        public Action<Type> OnAfterHide { get; set; }

        [field: SerializeField, ReadOnly] public bool IsShowing { get; protected set; } = false;
        [field: SerializeField, ReadOnly] public bool InQueue { get; set; } = false;
        [field: SerializeField, ReadOnly] public int QueuePriority { get; protected set; } = 0;

        public IWindowService WindowService { get; private set; }

        [Inject]
        protected virtual void Inject(IWindowService windowsService)
        {
            WindowService = windowsService;
        }

        protected virtual void Start()
        {
            WindowService.RegisterWindow(this);
            StartAction();
        }

        private void OnDestroy()
        {
            WindowService.UnregisterWindow(this);
            DestroyAction();
        }

        [Group("Buttons")]
        [Button("Show window")]
        public async UniTask Show()
        {
            OnBeforeShow?.Invoke(GetType());
            await ShowAction();
            IsShowing = true;
            OnAfterShow?.Invoke(GetType());
        }

        [Group("Buttons")]
        [Button("Hide window")]
        public async UniTask Hide()
        {
            OnBeforeHide?.Invoke(GetType());
            await HideAction();
            IsShowing = false;
            OnAfterHide?.Invoke(GetType());
        }

        public async void Toggle()
        {
            if (IsShowing)
                await Hide();
            else
                await Show();
        }

        protected virtual async void StartAction()
        {
            if (IsShowing || gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                await Hide();
            }
            else
                IsShowing = false;
        }

        protected virtual void DestroyAction()
        {
        }

        protected virtual UniTask ShowAction()
        {
            gameObject.SetActive(true);
            return default;
        }

        protected virtual UniTask HideAction()
        {
            gameObject.SetActive(false);
            return default;
        }
    }
}