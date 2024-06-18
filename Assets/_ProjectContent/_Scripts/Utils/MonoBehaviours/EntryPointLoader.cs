using Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils.MonoBehaviours
{
    public class EntryPointLoader : MonoBehaviour
    {
        private void Awake()
        {
            if (EntryPoint.IsAwakened)
                Destroy(gameObject);
            else
                SceneManager.LoadScene(0);
        }
    }
}