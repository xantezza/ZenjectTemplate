using UnityEngine.UI;

namespace Infrastructure.Services.Tutorial
{
    public interface ITutorialService
    {
        void LockToButton(Button button, bool autoHideOnClick = true);
        void Hide();
    }
}