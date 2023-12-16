using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Configs
{
    [Serializable]
    public class InfrastructureConfig : IConfig
    {
        [field: JsonProperty, SerializeField] public bool IsDebugEnabled1 { get; private set; } = false;
        [JsonProperty] public readonly bool IsDebugEnabled;
        [JsonProperty] public readonly float FakeTimeBeforeLoad = 5f;
        [JsonProperty] public readonly float FakeMinimalLoadTime;
        [JsonProperty] public readonly float FakeTimeAfterLoad = 0.5f;
    }
}