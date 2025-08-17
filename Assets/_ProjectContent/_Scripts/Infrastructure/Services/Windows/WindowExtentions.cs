namespace Infrastructure.Services.Windows
{
    public static class WindowExtensions
    {
        public static void PutInQueue(this IWindowBase window) =>
            window.WindowService.QueueController.AddWindowInQueue(window);

        public static void PutInQueue<TWindow>(this WindowBase<TWindow> window) where TWindow : class, IWindowBase =>
            window.WindowService.QueueController.AddWindowInQueue(window);
    }
}