using System;
using Configs;
using Configs.RemoteConfig;
using Newtonsoft.Json;
using TriInspector;
using UnityEngine;

namespace Editor.Utils
{
    public class ConfigUtility : ScriptableObject
    {
        private enum Type
        {
            Infrastructure
        }

        [SerializeField] private Type type;


        [ShowIf(nameof(isInfrastructure)), SerializeField] private InfrastructureConfig _infrastructureConfig;
        private bool isInfrastructure => type == Type.Infrastructure;
        
        #pragma warning disable 
        [SerializeField] private string _key;
        #pragma warning disable 
        
        [SerializeField] [TextArea(12, 9999)] private string _json;

        [Button] [PropertyOrder(-10)]
        private void ToJson()
        {
            switch (type)
            {
                case Type.Infrastructure:
                    _key = RemoteConfigType.InfrastructureConfig;
                    _json = JsonConvert.SerializeObject(_infrastructureConfig, Formatting.Indented);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        [Button] [PropertyOrder(-10)]
        private void FromJson()
        {
            switch (type)
            {
                case Type.Infrastructure:
                    _infrastructureConfig = JsonConvert.DeserializeObject<InfrastructureConfig>(_json);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}