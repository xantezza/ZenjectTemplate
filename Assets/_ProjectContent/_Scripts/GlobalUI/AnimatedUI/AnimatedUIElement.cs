using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GlobalUI.AnimatedUI
{
    public class AnimatedUIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private bool _hasEnableAnimation;
        [ShowIf(nameof(_hasEnableAnimation))] [SerializeField] private AnimatedUIElementAnimation _enableAnimation;

        [SerializeField] private bool _hasOnHoverAnimation;
        [ShowIf(nameof(_hasOnHoverAnimation))] [SerializeField] private AnimatedUIElementAnimation _onHoverAnimation;

        [SerializeField] private bool _hasOnClickAnimation;
        [ShowIf(nameof(_hasOnClickAnimation))] [SerializeField] private AnimatedUIElementAnimation _onClickAnimation;

        private void Awake()
        {
            if (_hasEnableAnimation) _enableAnimation.Init();
            if (_hasOnHoverAnimation) _onHoverAnimation.Init();
            if (_hasOnClickAnimation) _onClickAnimation.Init();
        }

        private void OnEnable()
        {
            if (_hasEnableAnimation) _enableAnimation.AnimateIn();
        }

        private void OnDisable()
        {
            _enableAnimation.Stop();
            _onHoverAnimation.Stop();
            _onClickAnimation.Stop();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_hasOnHoverAnimation) _onHoverAnimation.AnimateIn(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_hasOnHoverAnimation) _onHoverAnimation.AnimateOut();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_hasOnClickAnimation) _onClickAnimation.AnimateIn(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_hasOnClickAnimation) _onClickAnimation.AnimateOut();
        }
    }
}