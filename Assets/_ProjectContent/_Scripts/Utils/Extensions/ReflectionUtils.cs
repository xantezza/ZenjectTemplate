using System.Linq;

namespace Utils.Extensions
{
    public static class ReflectionUtils
    {
        public static string[] GetClassPropertyInfo(object o)
        {
            var properties = o.GetType().GetProperties();
            return properties.Select(x => x.GetValue(o)?.ToString()).ToArray();
        }
    }
}