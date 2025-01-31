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
        private readonly IConditionalLoggingService _conditionalLoggingService;

        [Inject]
        public UnityAnalyticsService(IConditionalLoggingService conditionalLoggingService)
        {
            _conditionalLoggingService = conditionalLoggingService;
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
            
            _conditionalLoggingService.Log($"{eventName} sent", LogTag.Analytics);

            AnalyticsService.Instance.RecordEvent(eventName);
        }

        public void SendEvent(string eventName, Dictionary<string, object> paramsDictionary)
        {
            if (!InitializeUnityServicesState.IsInitialized) return;

            var customEvent = new CustomEvent(eventName);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{eventName} sent");

            foreach (var (key, value) in paramsDictionary)
            {
                stringBuilder.AppendLine($"\nwith param: {key}: {value}");
                customEvent.Add(key, value);
            }

            _conditionalLoggingService.Log(stringBuilder.ToString(), LogTag.Analytics);
            AnalyticsService.Instance.RecordEvent(customEvent);
        }
    }
}