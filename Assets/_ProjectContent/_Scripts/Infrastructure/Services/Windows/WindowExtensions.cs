namespace Infrastructure.Services.Windows
{
    public static class WindowExtensions
    {
        public static void PutInQueue(this IWindowBase window) =>
            window.WindowService.QueueController.AddWindowInQueue(window);
    }
}