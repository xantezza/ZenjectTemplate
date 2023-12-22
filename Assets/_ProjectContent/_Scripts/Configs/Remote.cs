using System;
using System.Collections.Generic;
using Firebase.RemoteConfig;
using Infrastructure.Services.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Configs
{
    /// <summary>
    ///     Remote Settings
    /// </summary>
    public static class Remote
    {
        public static event Action OnInitializeAny;
        public static event Action OnInitializeDefault;
        public static event Action OnInitializeRemote;

        public static InfrastructureConfig InfrastructureConfig { get; private set; }

        private static IConditionalLoggingService _loggingService;
        private static IDictionary<string, JToken> _cachedDefaultConfig;
        private static IDictionary<string, JToken> _remoteConfig;

        private static bool _hasInitializedByRemote;

        public static void InitializeByDefault(IDictionary<string, JToken> cachedDefaultConfig, IConditionalLoggingService loggingService)
        {
            _cachedDefaultConfig = cachedDefaultConfig;

            if (_hasInitializedByRemote) return;

            ParseConfigs(loggingService);
            OnInitializeDefault?.Invoke();
            OnInitializeAny?.Invoke();
        }

        public static void InitializeByRemote(IDictionary<string, JToken> remoteConfig, IConditionalLoggingService loggingService)
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
            InfrastructureConfig = Parse<InfrastructureConfig>(ConfigType.InfrastructureConfig);
        }

        private static T Parse<T>(string type) where T : IConfig
        {
            try
            {
                return InternalParse(_remoteConfig ?? _cachedDefaultConfig);
            }
            catch (Exception e)
            {
                _loggingService.LogError($"Failed to parse remote config, using cached default. Exception: {e}", LogTag.RemoteSettings);

                return InternalParse(_cachedDefaultConfig);
            }

            T InternalParse(IDictionary<string, JToken> config)
            {
                var configString = config[type];

                _loggingService.Log($"{type}: {configString}", LogTag.RemoteSettings);

                return JsonConvert.DeserializeObject<T>(configString.ToString());
            }
        }
    }
}