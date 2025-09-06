using System.Collections.Generic;
using System.Text;
using Infrastructure.Services.Log;
using Infrastructure.StateMachines.InitializationStateMachine.States;
using Unity.Services.Analytics;
using UnityEngine;
using Zenject;
using Logger = Infrastructure.Services.Log.Logger;

namespace Infrastructure.Services.Analytics
{
    public class UnityAnalyticsService : IAnalyticsService
    {
        [Inject]
        public UnityAnalyticsService()
        {
            Application.focusChanged += OnApplicationFocus;
        }

        private void OnApplicationFocus(bool focusStatus)
        {
            if (focusStatus) return;
            if (!Application.isPlaying) return;
            if (!InitializeUnityServicesState.IsInitialized) return;

            AnalyticsService.Instance.Flush();
        }

        public void SendEvent(string eventName)
        {
            if (!InitializeUnityServicesState.IsInitialized) return;

#if DEV
            Logger.Log($"{eventName} sent", LogTag.Analytics);
            return;
#endif
            AnalyticsService.Instance.RecordEvent(eventName);
        }

        public void SendEvent(string eventName, Dictionary<string, object> paramsDictionary)
        {
            if (!InitializeUnityServicesState.IsInitialized) return;

            var customEvent = new CustomEvent(eventName);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{eventName} sent with params:");

            foreach (var (key, value) in paramsDictionary)
            {
                stringBuilder.AppendLine($"\n{key}: {value}");
                customEvent.Add(key, value);
            }

#if DEV
            Logger.Log(stringBuilder.ToString(), LogTag.Analytics);
            return;
#endif
            AnalyticsService.Instance.RecordEvent(customEvent);
        }

        public void SendEvent(string eventName, object data)
        {
            SendEvent(eventName, new Dictionary<string, object>() {["data"] = data});
        }
    }
}