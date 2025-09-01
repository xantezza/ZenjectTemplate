using System;
using Infrastructure.Services.Log;

namespace Infrastructure.Services.Windows.Queues
{
    public class WindowsQueueController
    {
        private readonly IWindowService _windowsService;
        private WindowsQueue _mainQueue;

        public WindowsQueueController(IWindowService windowsService)
        {
            _windowsService = windowsService;
            _mainQueue = new WindowsQueue(windowsService);
        }

        public void Next() => _mainQueue?.Next();

        public void RunQueue()
        {
            if (_mainQueue == null)
                _mainQueue = new WindowsQueue(_windowsService);
            if (_mainQueue.IsRunning)
                return;

            _mainQueue.OnQueueFinished += OnQueueFinished;

            if (_mainQueue.Run())
            {
                Logger.Log($"Windows queue is run!", LogTag.WindowsQueueController);
            }
            else
            {
                Logger.Error($"Windows queue run is aborted!", LogTag.WindowsQueueController);
            }
        }

        public void AddWindowsGroupInQueue(WindowsGroupBase windowsGroupBase)
        {
            foreach (Type windowType in windowsGroupBase.WindowTypes)
                AddWindowByTypeInQueue(windowType);
        }

        public void AddWindowByTypeInQueue(Type type)
        {
            if (_mainQueue.AddWindowByTypeIn(type))
            {
                Logger.Log($"A new window '{type.ToString().Split('.')[^1]}' has been successfully added to the window queue.", LogTag.WindowsQueueController);
            }
            else
            {
                Logger.Error($"Couldn't add window '{type.ToString().Split('.')[^1]}' was not added to the queue because the window does not exist in the context of the scene.", LogTag.WindowsQueueController);
            }
        }

        public void AddWindowInQueue(IWindowBase window)
        {
            if (_mainQueue.AddWindowIn(window))
            {
#if DEV
                Logger.Log($"A new window '{window.GetType().ToString().Split('.')[^1]}' has been successfully added to the window queue.", LogTag.WindowsQueueController);
#endif
            }
            else
            {
#if DEV
                Logger.Warn($"Couldn't add window '{window.GetType().ToString().Split('.')[^1]}' was not added to the queue because it already exists.", LogTag.WindowsQueueController);
#endif
            }
        }

        public void AddWindowInQueue<T>(WindowBase<T> window) where T : class, IWindowBase =>
            AddWindowInQueue(window);

        private void OnQueueFinished()
        {
            _mainQueue.OnQueueFinished -= OnQueueFinished;
#if DEV
            Logger.Log($"Windows queue is finished!", LogTag.WindowsQueueController);
#endif
        }
    }
}