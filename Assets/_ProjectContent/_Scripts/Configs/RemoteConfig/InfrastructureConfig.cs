using System;
using Newtonsoft.Json;

namespace Configs.RemoteConfig
{
    [Serializable]
    public class InfrastructureConfig : IConfig
    {
        [JsonProperty] public float FakeTimeBeforeLoad;
        [JsonProperty] public float FakeMinimalLoadTime = 0.2f;
        [JsonProperty] public float FakeTimeAfterLoad = 0.2f;
    }
}