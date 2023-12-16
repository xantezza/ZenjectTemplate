#if !ODIN_INSPECTOR

using UnityEditor;
using Zenject;

namespace Plugins.Zenject.Source.Editor.Editors
{
    [NoReflectionBaking]
    public class RunnableContextEditor : ContextEditor
    {
        SerializedProperty _autoRun;

        public override void OnEnable()
        {
            base.OnEnable();

            _autoRun = serializedObject.FindProperty("_autoRun");
        }

        protected override void OnGui()
        {
            base.OnGui();

            EditorGUILayout.PropertyField(_autoRun);
        }
    }
}


#endif
