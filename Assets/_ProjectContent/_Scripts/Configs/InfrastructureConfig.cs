using System;
using Newtonsoft.Json;

namespace Configs
{
    [Serializable]
    public class InfrastructureConfig : IConfig
    {
        [JsonProperty] public readonly float FakeTimeBeforeLoad = 5f;
        [JsonProperty] public readonly float FakeMinimalLoadTime;
        [JsonProperty] public readonly float FakeTimeAfterLoad = 0.5f;
    }
}