using UnityEngine.UI;

namespace Infrastructure.Services.Tutorial
{
    public interface ITutorialService
    {
        void LockToButton(Button button, bool animated = true, bool autoHideOnClick = true);
        void Hide();
    }
}