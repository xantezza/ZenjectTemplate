using System;
using Infrastructure.Services.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Configs.RemoteConfig
{
    public static class RemoteConfig
    {
        public static event Action OnInitializeAny;
        public static event Action OnInitializeDefault;
        public static event Action OnInitializeRemote;

        public static InfrastructureConfig Infrastructure { get; private set; }

        private static IConditionalLoggingService _loggingService;
        private static JToken _cachedDefaultConfig;
        private static JToken _remoteConfig;

        private static bool _hasInitializedByRemote;

        public static void InitializeByDefault(JToken cachedDefaultConfig, IConditionalLoggingService loggingService)
        {
            _cachedDefaultConfig = cachedDefaultConfig;

            if (_hasInitializedByRemote) return;

            ParseConfigs(loggingService);
            OnInitializeDefault?.Invoke();
            OnInitializeAny?.Invoke();
        }

        public static void InitializeByRemote(JToken remoteConfig, IConditionalLoggingService loggingService)
        {
            _hasInitializedByRemote = true;
            _remoteConfig = remoteConfig;
            ParseConfigs(loggingService);
            OnInitializeRemote?.Invoke();
            OnInitializeAny?.Invoke();
        }

        private static void ParseConfigs(IConditionalLoggingService loggingService)
        {
            _loggingService = loggingService;
            Infrastructure = Parse<InfrastructureConfig>(RemoteConfigType.InfrastructureConfig);
        }

        private static T Parse<T>(string type) where T : IConfig, new()
        {
            try
            {
                return InternalParse(_remoteConfig ?? _cachedDefaultConfig);
            }
            catch (Exception exception1)
            {
                try
                {
                    _loggingService.LogError($"Failed to parse remote config \"{type}\", using cached default. Exception: {exception1}", LogTag.UnityServices);

                    return InternalParse(_cachedDefaultConfig);
                }
                catch (Exception exception2)
                {
                    _loggingService.LogError($"Failed to parse default config \"{type}\", using scripted default. Exception: {exception2}", LogTag.UnityServices);
                    return new T();
                }
            }

            T InternalParse(JToken config)
            {
                var configString = config[type].ToString();

                _loggingService.Log($"{type}: {configString}", LogTag.UnityServices);

                return JsonConvert.DeserializeObject<T>(configString);
            }
        }
    }
}