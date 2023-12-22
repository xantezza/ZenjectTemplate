using UnityEngine;

namespace Utils.MonoBehaviours
{
    [ExecuteInEditMode]
    public class ChildLayerSetter : MonoBehaviour
    {
        private void OnTransformChildrenChanged()
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