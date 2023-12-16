using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

namespace Utils.Extensions
{
    public static class DOTweenExtensions
    {
        public static TweenerCore<Vector2, Vector2, VectorOptions> DOSize(this SpriteRenderer target, Vector2 endValue, float duration)
        {
            var t = DOTween.To(() => target.size, x => target.size = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        public static Tweener DOValueCounter(this TextMeshPro target, float fromValue, float endValue, float duration)
        {
            var t = DOTween.To(x => target.text = ((int) Mathf.Clamp(x, 0, endValue)).ToInvariantComasFormat(), fromValue, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        public static Tweener DOCustomCounter(this TextMeshPro target, float fromValue, float endValue, float duration, Func<float, string> applyFunction)
        {
            var t = DOTween.To(x => { target.text = applyFunction.Invoke(x); }, fromValue, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        public static Tweener DOPopOutScale(this Transform transform, Vector3 maxSize, Vector3 targetSize, float firstZoomDuration, float secondZoomDuration, Action finishedCallback = null)
        {
            var tween = transform.DOScale(maxSize, firstZoomDuration);
            tween.OnComplete(() => transform.DOScale(targetSize, secondZoomDuration).OnComplete(() => finishedCallback?.Invoke()));

            return tween;
        }
    }
}