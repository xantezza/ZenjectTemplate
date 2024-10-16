using System;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using TriInspector;
using UnityEngine;

namespace Infrastructure.Providers.DefaultConfigProvider
{
    public class CachedDefaultUnityRemoteConfigProvider : MonoBehaviour, IDefaultConfigProvider
    {
        private const int TIME_OF_LOSS_OF_RELEVANCE_IN_MINUTES = -5;

        [SerializeField] [ReadOnly] private string _lastFetchDate;
        [SerializeField] [ReadOnly] [UsedImplicitly] private string _currentDate;
        [SerializeField] [ReadOnly] private int _minutesFromLastFetch;
        [SerializeField] [TextArea(25, 99999)] [ReadOnly] private string _cachedConfigString;

        private JToken _cachedConfig;

        public JToken CachedConfig
        {
            get
            {
                if (_cachedConfig == null) _cachedConfig = JObject.Parse(_cachedConfigString);

                return _cachedConfig;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _currentDate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            var lastCachingDate = DateTime.Parse(_lastFetchDate);
            _minutesFromLastFetch = (int) lastCachingDate.Subtract(DateTime.Now).TotalMinutes;

            if (_minutesFromLastFetch < TIME_OF_LOSS_OF_RELEVANCE_IN_MINUTES) FetchDefaultConfig();
        }

        [Button] [PropertyOrder(-10)]
        private void FetchDefaultConfig()
        {
            Unity.RemoteConfig.Editor.RemoteConfigWebApiClient.fetchDefaultEnvironmentFinished += OnFetchDefaultEnvironmentID;
            Unity.RemoteConfig.Editor.RemoteConfigWebApiClient.FetchDefaultEnvironment(Application.cloudProjectId);
        }

        private void OnFetchDefaultEnvironmentID(string defaultEnvironmentId)
        {
            Unity.RemoteConfig.Editor.RemoteConfigWebApiClient.fetchConfigsFinished += OnFetchDefaultConfigFinished;
            Unity.RemoteConfig.Editor.RemoteConfigWebApiClient.FetchConfigs(Application.cloudProjectId, defaultEnvironmentId);
        }

        private void OnFetchDefaultConfigFinished(JObject defaultConfig)
        {
            var JObject = new JObject();

            for (var i = 0; i < defaultConfig["value"].Count(); i++) JObject.Add(defaultConfig["value"][i]["key"].ToString(), JObject.Parse(defaultConfig["value"][i]["value"].ToString()));

            _cachedConfigString = JObject.ToString();
            _lastFetchDate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            _minutesFromLastFetch = 0;
        }
#endif
    }
}