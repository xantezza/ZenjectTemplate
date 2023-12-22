using Cysharp.Threading.Tasks;

namespace Infrastructure.Services.Analytics
{
    public interface IAnalyticsLogService
    {
        UniTask Initialize();

        void LogEvent(string eventName);
    }
}