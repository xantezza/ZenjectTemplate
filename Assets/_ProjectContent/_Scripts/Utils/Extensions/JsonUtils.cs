using System;
using UnityEngine;

namespace Utils.Extensions
{
    public static class JsonUtils
    {
        public static bool TryDeserialize<T>(this string serialized, out T deserialized)
        {
            try
            {
                deserialized = JsonUtility.FromJson<T>(serialized);
                if (deserialized != null) return true;
                Debug.LogError("[JsonUtils] Fail TryDeserialize!");
                return false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JsonUtils] Fail TryDeserialize - {ex.Message}\r\n{ex.StackTrace}");
                deserialized = default;
                return false;
            }
        }

        public static string ToJson<T>(this T data)
        {
            return JsonUtility.ToJson(data);
        }

        public static bool IsJson(this string data)
        {
            try
            {
                if ((!data.StartsWith("{") || !data.EndsWith("}")) && (!data.StartsWith("[") || !data.EndsWith("]"))) return false;
                var result = JsonUtility.FromJson<object>(data);
                return result != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}