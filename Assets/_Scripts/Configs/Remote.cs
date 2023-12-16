using System;
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

            T InternalParse(JToken config)
            {
                var configString = config[type].ToString();

                _loggingService.Log($"{type}: {configString}", LogTag.RemoteSettings);

                return JsonConvert.DeserializeObject<T>(configString);
            }
        }
    }
}