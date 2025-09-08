using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Infrastructure.Services.Tutorial
{
    public class TutorialLockToButton : MonoBehaviour
    {
        [SerializeField] private Button _buttonToLock;
        private ITutorialService _tutorialService;

        [Inject]
        private void Inject(ITutorialService tutorialService)
        {
            _tutorialService = tutorialService;
        }

        private void Start()
        {
            _tutorialService.LockToButton(_buttonToLock);
        }
    }
}