using System;

namespace Utils.Extensions
{
    public static class EnumExtensions
    {
        public static bool IsObsolete(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (ObsoleteAttribute[])
                fi.GetCustomAttributes(typeof(ObsoleteAttribute), false);
            return attributes != null && attributes.Length > 0;
        }
    }
}