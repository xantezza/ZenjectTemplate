using System;
using Newtonsoft.Json;

namespace Configs.RemoteConfig
{
    [Serializable]
    public class InfrastructureConfig : IConfig
    {
        [JsonProperty] public float FakeMinimalLoadTime = 0.1f;
    }
}