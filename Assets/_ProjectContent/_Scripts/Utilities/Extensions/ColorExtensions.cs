using UnityEngine;

namespace Utilities.Extensions
{
    public static class ColorExtensions
    {
        public static string ToHEX(this Color color)
        {
            return ColorUtility.ToHtmlStringRGBA(color);
        }
        
        public static string Colorize(this Color color, object data)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{data}</color>";
        }
        public static string ColorizeWithBrackets(this Color color, object data)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>[{data}]</color>";
        }
    }
}