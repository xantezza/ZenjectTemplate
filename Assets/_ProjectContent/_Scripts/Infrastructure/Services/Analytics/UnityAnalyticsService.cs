using System.Collections.Generic;
using System.Text;
using Infrastructure.Services.Logging;
using Infrastructure.StateMachines.InitializationStateMachine.States;
using Unity.Services.Analytics;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.Analytics
{
    public class UnityAnalyticsService : IAnalyticsService
    {
        private readonly LoggingService _loggingService;

        [Inject]
        public UnityAnalyticsService(LoggingService loggingService)
        {
            _loggingService = loggingService;
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
            
            _loggingService.Log($"{eventName} sent", LogTag.Analytics);

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

            _loggingService.Log(stringBuilder.ToString(), LogTag.Analytics);
            AnalyticsService.Instance.RecordEvent(customEvent);
        }
    }
}