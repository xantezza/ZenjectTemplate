using System;
using System.Collections.Generic;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Windows.Queues;
using Zenject;

namespace Infrastructure.Services.Windows
{
    public interface IWindowService
    {
        public WindowsQueueController QueueController { get; }
        public HashSet<Type> ShowedWindows { get; }
        public void ShowWindow(Type type);
        public void HideWindow(Type type);
        public void ToggleWindow(Type type);
        public void ShowWindow<T>() where T : IWindowBase;
        public void HideWindow<T>() where T : IWindowBase;
        public void ToggleWindow<T>() where T : IWindowBase;
        public void RegisterWindow<T>(WindowBase<T> windowType) where T : IWindowBase;
        public void UnregisterWindow<T>(WindowBase<T> windowType) where T : IWindowBase;
        public TWindow GetWindow<TWindow>() where TWindow : class, IWindowBase;
        public IWindowBase GetWindow(Type type);
        public bool TryGetWindow<TWindow>(out TWindow window) where TWindow : class, IWindowBase;
        public bool TryGetWindow(Type type, out IWindowBase window);
        public bool ExistWindow(Type type);
    }

    public class WindowService : IWindowService
    {
        private readonly Dictionary<Type, IWindowBase> _windows = new();
        private readonly LoggingService _loggingService;
        public HashSet<Type> ShowedWindows { get; } = new();
        public WindowsQueueController QueueController { get; }

        [Inject]
        public WindowService(LoggingService loggingService)
        {
            _loggingService = loggingService;
            QueueController = new WindowsQueueController(this, loggingService);
        }

        public bool ExistWindow(Type type) =>
            _windows.ContainsKey(type);

        public TWindow GetWindow<TWindow>() where TWindow : class, IWindowBase
        {
            if (_windows.TryGetValue(typeof(TWindow), out var window))
                return window as TWindow;
            return null;
        }

        public IWindowBase GetWindow(Type type) =>
            _windows.GetValueOrDefault(type);

        public bool TryGetWindow<TWindow>(out TWindow window) where TWindow : class, IWindowBase
        {
            if (_windows.TryGetValue(typeof(TWindow), out var value))
            {
                window = value as TWindow;
                return true;
            }

            window = null;
            return false;
        }

        public bool TryGetWindow(Type type, out IWindowBase window) =>
            _windows.TryGetValue(type, out window);

        public async void ShowWindow(Type type)
        {
            if (_windows.TryGetValue(type, out var window))
                await window.Show();
        }

        public async void HideWindow(Type type)
        {
            if (_windows.TryGetValue(type, out var window))
                await window.Hide();
        }

        public void ToggleWindow(Type type)
        {
            if (_windows.TryGetValue(type, out var window))
                window.Toggle();
        }

        public void ShowWindow<T>() where T : IWindowBase => ShowWindow(typeof(T));

        public void HideWindow<T>() where T : IWindowBase => HideWindow(typeof(T));
        public void ToggleWindow<T>() where T : IWindowBase => ToggleWindow(typeof(T));

        public void RegisterWindow<T>(WindowBase<T> window) where T : IWindowBase
        {
            _windows.Add(typeof(T), window);
            window.OnAfterHide += OnAfterWindowHide;
            window.OnAfterShow += OnAfterShow;
            _loggingService.Log($"Registering window of type {typeof(T).ToString().Split('.')[^1]}", LogTag.WindowService);
        }

        public void UnregisterWindow<T>(WindowBase<T> window) where T : IWindowBase
        {
            _windows.Remove(typeof(T));
            window.OnAfterHide -= OnAfterWindowHide;
            window.OnAfterShow -= OnAfterShow;
            _loggingService.Log($"Unregistering window of type {typeof(T).ToString().Split('.')[^1]}", LogTag.WindowService);
        }

        private void OnAfterShow(Type windowType) =>
            ShowedWindows.Add(windowType);

        private void OnAfterWindowHide(Type windowType) =>
            ShowedWindows.Remove(windowType);
    }
}