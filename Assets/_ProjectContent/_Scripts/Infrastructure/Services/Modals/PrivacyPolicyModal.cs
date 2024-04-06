using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Services.Modals
{
    public class PrivacyPolicyModal : ModalPopup
    {
        private const string PRIVACY_POLICY_LINK = "https://www.privacypolicies.com/live/0e89fdf4-74ab-472a-9fe8-390a4a854a77";

        [SerializeField] private Button _showPolicyButton;

        protected override void OnEnable()
        {
            base.OnEnable();
            _showPolicyButton.onClick.AddListener(ShowPolicy);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _showPolicyButton.onClick.RemoveListener(ShowPolicy);
        }

        private void ShowPolicy()
        {
            Application.OpenURL(PRIVACY_POLICY_LINK);
        }
    }
}