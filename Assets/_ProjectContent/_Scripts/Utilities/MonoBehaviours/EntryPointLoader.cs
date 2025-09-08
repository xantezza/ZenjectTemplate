using Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities.MonoBehaviours
{
    public class EntryPointLoader : MonoBehaviour
    {
        private void Awake()
        {
            if (!EntryPoint.HasStarted) SceneManager.LoadScene(0);
        }
    }
}