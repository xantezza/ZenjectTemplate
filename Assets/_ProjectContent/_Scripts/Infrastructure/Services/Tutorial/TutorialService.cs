using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Infrastructure.Services.Tutorial
{
    public class TutorialService : MonoBehaviour, ITutorialService
    {
        [SerializeField] private TutorialScreenBlocker _tutorialScreenBlocker;
        
        public void LockToButton(Button button, bool autoHideOnClick = true)
        {
            ProcessRect(button.GetComponent<RectTransform>());

            void HideFromButton()
            {
                button.onClick.RemoveListener(HideFromButton);
                Hide();
            }

            if (autoHideOnClick) button.onClick.AddListener(HideFromButton);
        }

        public void Hide()
        {
            _tutorialScreenBlocker.gameObject.SetActive(false);
        }

        private void ProcessRect(RectTransform rectTransform)
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

            _tutorialScreenBlocker.Show(new Vector2(holeCenterX, holeCenterY), new Vector2(holeSizeX, holeSizeY));
        }
    }
}