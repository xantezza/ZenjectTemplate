using System;
using Newtonsoft.Json;

namespace Configs.RemoteConfig.Configs
{
    [Serializable]
    public class InfrastructureConfig : IConfig
    {
        [JsonProperty] public float FakeMinimalLoadTime = 0.1f;
    }
}