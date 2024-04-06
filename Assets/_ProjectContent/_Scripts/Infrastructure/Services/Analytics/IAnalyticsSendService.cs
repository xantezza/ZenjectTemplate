using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Infrastructure.Services.Analytics
{
    public interface IAnalyticsSendService
    {
        void SendEvent(string eventName);
        void SendEvent(string eventName, Dictionary<string, object> paramsDictionary);
    }
}