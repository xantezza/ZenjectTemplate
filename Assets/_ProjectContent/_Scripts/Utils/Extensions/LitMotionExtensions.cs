using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace Utils.Extensions
{
    public static class LitMotionExtensions
    {
        public static async UniTask<MotionHandle> DOPopOutScale(this Transform transform, float maxSize, float targetSize, float firstZoomDuration, float secondZoomDuration)
        {
            var motionHandle = LMotion.Create(transform.localScale, Vector3.one * maxSize, firstZoomDuration)
                .BindToLocalScale(transform);

            await motionHandle.ToUniTask();
            
            motionHandle = LMotion.Create(transform.localScale, Vector3.one * targetSize, secondZoomDuration)
                .BindToLocalScale(transform);

            return motionHandle;
        }
    }
}