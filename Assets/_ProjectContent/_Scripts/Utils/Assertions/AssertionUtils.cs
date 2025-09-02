using System;

namespace Utils.Assertions
{
    public static class AssertionUtils
    {
        public static T AssertNotNull<T>(this T obj) where T : class
        {
            if (obj == null)
            {
                throw new NullReferenceException($"Object of type {nameof(T)} is null");
            }
            return obj;
        }
    }
}