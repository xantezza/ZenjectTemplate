using UnityEngine;
using UnityEngine.UI;
using TriInspector;

namespace Infrastructure.Services.Tutorial
{
    public class TutorialService : MonoBehaviour, ITutorialService
    {
        [SerializeField] private TutorialScreenBlocker _tutorialScreenBlocker;

        private void Start()
        {
            Hide();
        }

        [Button]
        public void LockToButton(Button button, bool animated = true, bool autoHideOnClick = true)
        {
            var targetState = ProcessRect(button.GetComponent<RectTransform>());

            if (animated) _tutorialScreenBlocker.ShowAnimated(targetState);
            else _tutorialScreenBlocker.Show(targetState);

            if (autoHideOnClick) button.onClick.AddListener(HideFromButton);

            void HideFromButton()
            {
                button.onClick.RemoveListener(HideFromButton);
                Hide();
            }
        }

        [Button]
        public void Hide()
        {
            _tutorialScreenBlocker.gameObject.SetActive(false);
        }

        private Vector4 ProcessRect(RectTransform rectTransform)
        {
            _tutorialScreenBlocker.gameObject.SetActive(true);
            var blockerRect = _tutorialScreenBlocker.GetComponent<RectTransform>();

            var buttonWorldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(buttonWorldCorners);

            var localCorners = new Vector3[4];
            for (int i = 0; i < 4; i++)
            {
                localCorners[i] = blockerRect.InverseTransformPoint(buttonWorldCorners[i]);
            }

            var bottomLeft = localCorners[0];
            var topRight = localCorners[2];

            var localCenter = (bottomLeft + topRight) * 0.5f;
            var localSize = topRight - bottomLeft;

            var rect = blockerRect.rect;

            var holeCenterX = (localCenter.x - rect.x) / rect.width;
            var holeCenterY = (localCenter.y - rect.y) / rect.height;

            var holeSizeX = localSize.x / rect.width;
            var holeSizeY = localSize.y / rect.height;
            
            return new Vector4(holeCenterX, holeCenterY, holeSizeX, holeSizeY);
        }
    }
}