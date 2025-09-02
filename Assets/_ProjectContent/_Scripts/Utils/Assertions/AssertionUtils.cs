using System;
using Infrastructure.Services.Log;

namespace Utils.Assertions
{
    public static class AssertionUtils
    {
        public static T AssertNotNull<T>(this T obj) where T : class
        {
            if (obj == null)
            {
                Logger.Error($"Object of type {nameof(T)} is null");
            }
            return obj;
        }
    }
}