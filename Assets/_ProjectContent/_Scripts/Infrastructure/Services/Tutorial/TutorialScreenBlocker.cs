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

        protected override void Awake()
        {
            base.Awake();
            
            if (material != null)
                _material = Instantiate(material);
            else
                _material = new Material(Shader.Find("UI/RectHoleMask"));

            material = _material;
        }

        public void Show(Vector2 center, Vector2 size)
        {
            _holeCenterX = center.x;
            _holeCenterY = center.y;
            _holeSizeX = size.x;
            _holeSizeY = size.y;
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