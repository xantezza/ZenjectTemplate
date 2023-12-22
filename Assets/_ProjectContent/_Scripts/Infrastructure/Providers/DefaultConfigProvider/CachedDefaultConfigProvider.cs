using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TriInspector;
using UnityEngine;

namespace Infrastructure.Providers.DefaultConfigProvider
{
    public class CachedDefaultConfigProvider : MonoBehaviour, IDefaultConfigProvider
    {
        private const int TIME_OF_LOSS_OF_RELEVANCE_IN_MINUTES = -5;

        [SerializeField] [ReadOnly] private string _lastFetchDate;
        [SerializeField] [ReadOnly] [UsedImplicitly] private string _currentDate;
        [SerializeField] [ReadOnly] private int _minutesFromLastFetch;
        [SerializeField] [ReadOnly] [TextArea(25, 9999)] private string _cachedConfigString;

        private Dictionary<string, JToken> _cachedConfig;

        public IDictionary<string, JToken> CachedConfig =>
            _cachedConfig ??= JsonConvert
                .DeserializeObject<Dictionary<string, JToken>>(_cachedConfigString);

#if UNITY_EDITOR
        private void OnValidate()
        {
            _currentDate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            var lastCachingDate = DateTime.Parse(_lastFetchDate);
            _minutesFromLastFetch = (int) lastCachingDate.Subtract(DateTime.Now).TotalMinutes;

            if (_minutesFromLastFetch < TIME_OF_LOSS_OF_RELEVANCE_IN_MINUTES)
            {
                FetchConfig();
            }
        }

        [Button]
        private void FetchConfig()
        {
            var fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

        private void FetchComplete(Task fetchTask)
        {
            if (!fetchTask.IsCompleted)
            {
                Debug.LogError("Retrieval hasn't finished.");
                return;
            }

            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            using var info = remoteConfig.Info;
            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
                return;
            }

            remoteConfig.ActivateAsync().ContinueWithOnMainThread(Continuation);

            void Continuation(Task<bool> task)
            {
                var cache = FirebaseRemoteConfig.DefaultInstance.AllValues.ToDictionary(
                    keyValuePair => keyValuePair.Key,
                    keyValuePair => JToken.Parse(keyValuePair.Value.StringValue));

                _cachedConfigString = JsonConvert.SerializeObject(cache, Formatting.Indented);
                _minutesFromLastFetch = 0;
                _lastFetchDate = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            }
        }
#endif
    }
}