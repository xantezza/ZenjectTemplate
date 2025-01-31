using System;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GlobalUI.AnimatedUI
{
    [Serializable]
    public class AnimatedUIElementAnimation
    {
        [field: Header("Text")]
        [field: SerializeField] public bool HasTextAnimation { get; private set; }

        [ShowIf(nameof(HasTextAnimation))] [SerializeField] private TMP_Text _textLabel;
        [ShowIf(nameof(HasTextAnimation))] [SerializeField] private Color _textFillColor = Color.white;
        [ShowIf(nameof(HasTextAnimation))] [SerializeField] private float _textFontSizeDelta = 1f;
        [ShowIf(nameof(HasTextAnimation))] [SerializeField] private float _textCharacterSpacing = 12f;
        [ShowIf(nameof(HasTextAnimation))] [SerializeField] private float _textAnimationDuration = 0.2f;
        [ShowIf(nameof(HasTextAnimation))] [SerializeField] private Ease _textAnimationEase = Ease.OutSine;
        private Color _textInitialColor;
        private CompositeMotionHandle _textMotionHandles = new();

        [field: Header("Rect")]
        [field: SerializeField] public bool HasRectTransformAnimation;

        [ShowIf(nameof(HasRectTransformAnimation))] [SerializeField] private RectTransform _rectTransform;
        [ShowIf(nameof(HasRectTransformAnimation))] [SerializeField] private Vector3 _rectTransformTargetNewScale = new(0.9f, 0.9f, 0.9f);
        [ShowIf(nameof(HasRectTransformAnimation))] [SerializeField] private float _rectTransformAnimationDuration = 0.2f;
        [ShowIf(nameof(HasRectTransformAnimation))] [SerializeField] private Ease _rectTransformAnimationEase = Ease.OutSine;
        private Vector3 _rectTransformTargetInitialScale;
        private CompositeMotionHandle _rectMotionHandles = new();

        [field: Header("Fill")]
        [field: SerializeField] public bool HasFillImageAnimation { get; private set; }

        [ShowIf(nameof(HasFillImageAnimation))] [SerializeField] private Image _fillImage;
        [ShowIf(nameof(HasFillImageAnimation))] [SerializeField] private Color _fillImageColor = Color.white;
        [ShowIf(nameof(HasFillImageAnimation))] [SerializeField] private float _fillImageAnimationDuration = 0.2f;
        [ShowIf(nameof(HasFillImageAnimation))] [SerializeField] private Ease _fillImageAnimationEase = Ease.OutSine;
        private Color _fillImageInitialColor;
        private CompositeMotionHandle _fillImageMotionHandles = new();

        [field: Header("Hover")]
        [field: SerializeField] public bool HasHoverImageAnimation { get; private set; }

        [ShowIf(nameof(HasHoverImageAnimation))] [SerializeField] private Image _hoverImage;
        [ShowIf(nameof(HasHoverImageAnimation))] [SerializeField] private Color _hoverImageFillColor = Color.white;
        [ShowIf(nameof(HasHoverImageAnimation))] [SerializeField] private float _hoverImageAnimationDuration = 0.2f;
        [ShowIf(nameof(HasHoverImageAnimation))] [SerializeField] private Ease _hoverImageAnimationEase = Ease.OutSine;
        private Color _hoverImageInitialColor;
        private CompositeMotionHandle _hoverImageMotionHandles = new();

        public void Init()
        {
            if (HasTextAnimation) _textInitialColor = _textLabel.color;
            if (HasHoverImageAnimation) _hoverImageInitialColor = _hoverImage.color;
            if (HasFillImageAnimation) _fillImageInitialColor = _fillImage.color;
            if (HasRectTransformAnimation) _rectTransformTargetInitialScale = _rectTransform.localScale;
        }

        public void AnimateIn()
        {
            if (HasTextAnimation) AnimateTextIn();
            if (HasRectTransformAnimation) AnimateRectTransformIn();
            if (HasHoverImageAnimation) AnimateHoverImageIn();
        }

        public void AnimateIn(PointerEventData pointerEventData)
        {
            if (HasTextAnimation) AnimateTextIn();
            if (HasRectTransformAnimation) AnimateRectTransformIn();
            if (HasHoverImageAnimation) AnimateHoverImageIn();
            if (HasFillImageAnimation) AnimateFillImageIn(pointerEventData);
        }

        public void AnimateOut()
        {
            if (HasTextAnimation) AnimateTextOut();
            if (HasRectTransformAnimation) AnimateRectTransformOut();
            if (HasHoverImageAnimation) AnimateHoverImageOut();
            if (HasFillImageAnimation) AnimateFillImageOut();
        }

        public void Stop()
        {
            _textMotionHandles.Cancel();
            _rectMotionHandles.Cancel();
            _hoverImageMotionHandles.Cancel();
            _fillImageMotionHandles.Cancel();
        }

        private void AnimateTextIn()
        {
            _textMotionHandles.Cancel();

            LMotion.Create(_textLabel.fontSize, _textLabel.fontSize + _textFontSizeDelta, _textAnimationDuration)
                .WithEase(_textAnimationEase)
                .BindToFontSize(_textLabel)
                .AddTo(_textMotionHandles);

            LMotion.Create(_textLabel.color, _textFillColor, _textAnimationDuration)
                .WithEase(_textAnimationEase)
                .BindToColor(_textLabel)
                .AddTo(_textMotionHandles);

            LMotion.Create(_textLabel.characterSpacing, _textCharacterSpacing, _textAnimationDuration)
                .WithEase(_textAnimationEase)
                .BindWithState(_textLabel, (x, label) => { label.characterSpacing = x; })
                .AddTo(_textMotionHandles);
        }

        private void AnimateRectTransformIn()
        {
            _rectMotionHandles.Cancel();

            LMotion.Create(_rectTransform.localScale, _rectTransformTargetNewScale, _rectTransformAnimationDuration)
                .WithEase(_rectTransformAnimationEase)
                .BindToLocalScale(_rectTransform)
                .AddTo(_rectMotionHandles);
        }

        private void AnimateHoverImageIn()
        {
            _hoverImageMotionHandles.Cancel();

            LMotion.Create(_hoverImage.color, _hoverImageFillColor, _hoverImageAnimationDuration)
                .WithEase(_hoverImageAnimationEase)
                .BindToColor(_hoverImage)
                .AddTo(_hoverImageMotionHandles);
        }

        private void AnimateFillImageIn(PointerEventData pointerEventData)
        {
            _fillImageMotionHandles.Cancel();

            _fillImage.rectTransform.position = pointerEventData.position;

            LMotion.Create(_fillImage.color, _fillImageColor, _fillImageAnimationDuration)
                .WithEase(_fillImageAnimationEase)
                .BindToColor(_fillImage)
                .AddTo(_fillImageMotionHandles);

            LMotion.Create(Vector3.zero, Vector3.one, _fillImageAnimationDuration)
                .WithEase(_fillImageAnimationEase)
                .BindToLocalScale(_fillImage.transform)
                .AddTo(_fillImageMotionHandles);
        }

        private void AnimateTextOut()
        {
            _textMotionHandles.Cancel();

            LMotion.Create(_textLabel.fontSize, _textLabel.fontSize - _textFontSizeDelta, _textAnimationDuration)
                .WithEase(_textAnimationEase)
                .BindToFontSize(_textLabel)
                .AddTo(_textMotionHandles);

            LMotion.Create(_textLabel.color, _textInitialColor, _textAnimationDuration)
                .WithEase(_textAnimationEase)
                .BindToColor(_textLabel)
                .AddTo(_textMotionHandles);

            LMotion.Create(_textLabel.characterSpacing, 0, _textAnimationDuration)
                .WithEase(_textAnimationEase)
                .BindWithState(_textLabel, (x, label) => { label.characterSpacing = x; })
                .AddTo(_textMotionHandles);
        }

        private void AnimateRectTransformOut()
        {
            _rectMotionHandles.Cancel();

            LMotion.Create(_rectTransform.localScale, _rectTransformTargetInitialScale, _rectTransformAnimationDuration)
                .WithEase(_rectTransformAnimationEase)
                .BindToLocalScale(_rectTransform)
                .AddTo(_rectMotionHandles);
        }

        private void AnimateHoverImageOut()
        {
            _hoverImageMotionHandles.Cancel();

            LMotion.Create(_hoverImage.color, _hoverImageInitialColor, _hoverImageAnimationDuration)
                .WithEase(_hoverImageAnimationEase)
                .BindToColor(_hoverImage)
                .AddTo(_hoverImageMotionHandles);
        }

        private void AnimateFillImageOut()
        {
            _fillImageMotionHandles.Cancel();

            LMotion.Create(_fillImage.color, _fillImageInitialColor, _fillImageAnimationDuration)
                .WithEase(_fillImageAnimationEase)
                .BindToColor(_fillImage)
                .AddTo(_fillImageMotionHandles);

            LMotion.Create(Vector3.one, Vector3.zero, _fillImageAnimationDuration)
                .WithEase(_fillImageAnimationEase)
                .BindToLocalScale(_fillImage.transform)
                .AddTo(_fillImageMotionHandles);
        }
    }
}