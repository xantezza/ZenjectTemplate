using UnityEngine;

namespace UniRx
{
    internal static class YieldInstructionCache
    {
        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new();
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new();
    }
}