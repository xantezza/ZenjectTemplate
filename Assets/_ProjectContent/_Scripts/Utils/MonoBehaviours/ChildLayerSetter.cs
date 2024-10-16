using UnityEngine;

namespace Utils.MonoBehaviours
{
    public class ChildLayerSetter : MonoBehaviour
    {
        private void OnValidate()
        {
            ChangeChildrenLayer();
        }

        private void ChangeChildrenLayer()
        {
            var childrenTransforms = GetComponentsInChildren<Transform>();
            foreach (var childrenTransform in childrenTransforms) childrenTransform.gameObject.layer = gameObject.layer;
        }
    }
}