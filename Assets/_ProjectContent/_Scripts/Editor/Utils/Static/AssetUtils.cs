using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.Utils.Static
{
    public static class AssetUtils
    {
        public static T GetAssetOfType<T>() where T : Object
        {
            var assets = AssetDatabase.FindAssets($"t:{typeof(T).Name}")
                .Select(guid =>
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    return AssetDatabase.LoadAssetAtPath<T>(path);
                })
                .ToArray();

            if (assets.Length == 0)
            {
                Debug.LogWarning($"{typeof(T).Name} not found.");
                return default;
            }

            if (assets.Length > 1) Debug.LogWarning($"More than 1 {typeof(T).Name} founded. Get first found!");

            return assets.First();
        }
    }
}