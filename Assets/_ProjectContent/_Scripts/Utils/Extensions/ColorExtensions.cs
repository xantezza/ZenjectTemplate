using UnityEngine;

namespace Utils.Extensions
{
    public static class ColorExtensions
    {
        public static string ToHEX(this Color color)
        {
            return ColorUtility.ToHtmlStringRGBA(color);
        }
        
        public static string ToRichHEX(this Color color, object data)
        {
            return $"<color=#{color.ToHEX()}>{data}</color>";
        }
    }
}