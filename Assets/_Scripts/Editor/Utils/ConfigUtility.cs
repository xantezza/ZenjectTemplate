﻿using System;
using Configs;
using Newtonsoft.Json;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Editor.Utils
{
    [CreateAssetMenu]
    public class ConfigUtility : ScriptableObject
    {
        private enum Type
        {
            Infrastructure
        }

        [SerializeField] private Type type;


        [ShowIf(nameof(condition)), SerializeField] private InfrastructureConfig _infrastructureConfig;
        private bool condition => type == Type.Infrastructure;


        [SerializeField] [TextArea(12, 9999)] private string _serialized;

        [Button] [PropertyOrder(-10)]
        private void ToJson()
        {
            switch (type)
            {
                case Type.Infrastructure:
                    _serialized = JsonConvert.SerializeObject(_infrastructureConfig, Formatting.Indented);
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
                    _infrastructureConfig = JsonConvert.DeserializeObject<InfrastructureConfig>(_serialized);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        [MenuItem("Services/Config Serialization Window", false, 10)]
        private static void OpenWindow()
        {
            Selection.activeObject = Resources.FindObjectsOfTypeAll<ConfigUtility>()[0];
        }
    }
}