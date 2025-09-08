using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Infrastructure.Services.Windows.Queues
{
    public class WindowsQueue
    {
        public Action OnQueueFinished { get; set; }
        public Action OnQueueRunned { get; set; }
        public bool IsRunning { get; private set; }
        private readonly IWindowsService WindowsService;
        private readonly Dictionary<int, Queue<IWindowBase>> _queueItems = new();
        private int _currentQueueIndex;

        public WindowsQueue(IWindowsService windowsService)
        {
            WindowsService = windowsService;
        }

        public bool AddWindowByTypeIn(Type type)
        {
            if (WindowsService.TryGetWindow(type, out IWindowBase window))
                return AddWindowIn(window);
            return false;
        }

        public bool AddWindowIn(IWindowBase window)
        {
            ValidatePriority(window.QueuePriority);
            if (ExistWindowInQueue(window))
                return false;
            _queueItems[window.QueuePriority].Enqueue(window);
            if (IsRunning)
                UpdateQueue(window.QueuePriority);
            else
                WindowsService.QueueController.RunQueue();
            return true;
        }

        public bool AddWindowIn<T>(WindowBase<T> window) where T : class, IWindowBase => AddWindowIn((IWindowBase) window);

        public bool ExistWindowInQueue(IWindowBase window)
        {
            return _queueItems[window.QueuePriority].Contains(window);
        }

        public void ValidatePriority(int priority)
        {
            if (!_queueItems.ContainsKey(priority))
                _queueItems.Add(priority, new Queue<IWindowBase>());
        }

        public async void UpdateQueue(int priority)
        {
            if (_currentQueueIndex < priority)
            {
                UnSubscribeWindow();
                await _queueItems[_currentQueueIndex].Peek().Hide();
                _currentQueueIndex = priority;
                Start();
            }
        }

        public void GoToNextPriorityGroup()
        {
            _queueItems.Remove(_currentQueueIndex);
            if (_queueItems.Keys.Count == 0)
                Stop();
            else
            {
                _currentQueueIndex = _queueItems.Keys.Max();
                Start();
            }
        }

        public bool Run()
        {
            _currentQueueIndex = _queueItems.Keys.Max();
            Start();
            IsRunning = true;
            OnQueueRunned?.Invoke();
            return true;
        }

        private async void Start()
        {
            SubscribeWindow();
            await _queueItems[_currentQueueIndex].Peek().Show();
        }

        public async void Next(Type type = null)
        {
            UnSubscribeWindow();
            _queueItems[_currentQueueIndex].Dequeue();
            if (_queueItems[_currentQueueIndex].IsEmpty())
            {
                GoToNextPriorityGroup();
                return;
            }

            SubscribeWindow();
            var queueItem = _queueItems[_currentQueueIndex].Peek();
            await queueItem.Show();
            queueItem.InQueue = false;
        }

        public void Stop()
        {
            Clear();
            OnQueueFinished?.Invoke();
        }

        private void Clear()
        {
            _currentQueueIndex = 0;
            _queueItems.Clear();
            IsRunning = false;
        }

        private void SubscribeWindow() => _queueItems[_currentQueueIndex].Peek().OnAfterHide += Next;
        private void UnSubscribeWindow() => _queueItems[_currentQueueIndex].Peek().OnAfterHide -= Next;
    }
}