using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Services.Modals
{
    public class ErrorModal : ModalPopup
    {
        [SerializeField] private TMP_Text _errorTextLabel;
        [SerializeField] private TMP_Text _stackTraceTextLabel;
        [SerializeField] private Button _copyButton;
        private string _errorText;

        protected override void OnEnable()
        {
            base.OnEnable();
            _copyButton.onClick.AddListener(SetToBuffer);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _copyButton.onClick.RemoveListener(SetToBuffer);
        }

        public void Init(string condition, string stacktrace)
        {
            _errorText = $"{condition}\n{stacktrace}";
            _errorTextLabel.SetText(condition);
            _stackTraceTextLabel.SetText(stacktrace);
        }

        private void SetToBuffer()
        {
            GUIUtility.systemCopyBuffer = _errorText;
        }
    }
}