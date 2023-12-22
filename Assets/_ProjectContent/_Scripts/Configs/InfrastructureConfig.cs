using System;
using Newtonsoft.Json;

namespace Configs
{
    [Serializable]
    public class InfrastructureConfig : IConfig
    {
        [JsonProperty] public readonly float FakeTimeBeforeLoad = 0;
        [JsonProperty] public readonly float FakeMinimalLoadTime = 0.2f;
        [JsonProperty] public readonly float FakeTimeAfterLoad = 0.2f;
    }
}