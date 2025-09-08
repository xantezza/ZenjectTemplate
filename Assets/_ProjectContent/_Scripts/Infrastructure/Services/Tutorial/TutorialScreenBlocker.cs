using LitMotion;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Services.Tutorial
{
    public class TutorialScreenBlocker : Image
    {
        private float _holeCenterX = 0.5f;
        private float _holeCenterY = 0.5f;
        private float _holeSizeX = 0.3f;
        private float _holeSizeY = 0.2f;

        private Material _material;
        private bool _lockedByAnimation;
        private MotionHandle? _animation;
        protected override void Awake()
        {
            base.Awake();
            
            if (material != null)
                _material = Instantiate(material);
            else
                _material = new Material(Shader.Find("UI/RectHoleMask"));

            material = _material;
        }

        public void Show(Vector4 targetState)
        {
            _animation?.TryCancel();
            _lockedByAnimation = false;
            _holeCenterX = targetState.x;
            _holeCenterY = targetState.y;
            _holeSizeX = targetState.z;
            _holeSizeY = targetState.w;
        }

        public void ShowAnimated(Vector4 targetState, float time = 0.5f)
        {
            _animation?.TryCancel();
            _lockedByAnimation = true;
            var currentState = new Vector4(_holeCenterX, _holeCenterY, _holeSizeX, _holeSizeY);
            _animation = LMotion.Create(currentState, targetState, time)
                .WithOnComplete(() => { _lockedByAnimation = false;})
                .WithOnCancel(() => { _lockedByAnimation = false;})
                .Bind(AssignAnimatedValues);


        }

        private void AssignAnimatedValues(Vector4 vector4)
        {
            _holeCenterX = vector4.x;
            _holeCenterY = vector4.y;
            _holeSizeX = vector4.z;
            _holeSizeY = vector4.w;
        }

        private void Update()
        {
            if (_material == null)
                return;

            Vector4 holeCenter = new Vector4(_holeCenterX, _holeCenterY, 0, 0);
            Vector4 holeSize = new Vector4(_holeSizeX, _holeSizeY, 0, 0);

            _material.SetVector("_HoleCenter", holeCenter);
            _material.SetVector("_HoleSize", holeSize);
        }

        public override bool Raycast(Vector2 sp, Camera eventCamera)
        {
            if (!base.Raycast(sp, eventCamera))
                return false;
            if (_lockedByAnimation) return false;
            
            Rect rect = rectTransform.rect;

            Vector2 localPoint;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out localPoint))
                return false;

            Vector2 uv = new Vector2(
                (localPoint.x - rect.x) / rect.width,
                (localPoint.y - rect.y) / rect.height);

            Vector2 holeMin = new Vector2(_holeCenterX - _holeSizeX * 0.5f, _holeCenterY - _holeSizeY * 0.5f);
            Vector2 holeMax = new Vector2(_holeCenterX + _holeSizeX * 0.5f, _holeCenterY + _holeSizeY * 0.5f);

            if (uv.x >= holeMin.x && uv.x <= holeMax.x &&
                uv.y >= holeMin.y && uv.y <= holeMax.y)
            {
                return false;
            }

            return true;
        }
    }
}