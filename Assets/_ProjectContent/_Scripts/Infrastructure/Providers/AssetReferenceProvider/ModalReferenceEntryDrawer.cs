#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.Modals;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.Providers.AssetReferenceProvider
{
    [CustomPropertyDrawer(typeof(ModalReferenceEntry))]
    public class ModalReferenceEntryDrawer : PropertyDrawer
    {
        private static List<Type> modalTypes;
        private static string[] modalTypeNames;

        private const float lineHeight = 18f;
        private const float padding = 2f;

        private void InitTypes()
        {
            if (modalTypes != null)
                return;

            modalTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly =>
                {
                    try
                    {
                        return assembly.GetTypes();
                    }
                    catch
                    {
                        return new Type[0];
                    }
                })
                .Where(t => !t.IsAbstract && typeof(ModalPopup).IsAssignableFrom(t))
                .OrderBy(t => t.Name)
                .ToList();

            modalTypeNames = modalTypes.Select(t => t.FullName).ToArray();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            InitTypes();

            var typeNameProp = property.FindPropertyRelative("typeName");
            var assetRefProp = property.FindPropertyRelative("assetReference");

            position.height = lineHeight;
            EditorGUI.LabelField(position, label);

            position.y += lineHeight + padding;

            string currentTypeName = typeNameProp.stringValue;
            int currentIndex = Array.IndexOf(modalTypeNames, currentTypeName);
            if (currentIndex < 0) currentIndex = 0;

            int newIndex = EditorGUI.Popup(position, "Modal Type", currentIndex, modalTypeNames);

            if (newIndex != currentIndex)
            {
                typeNameProp.stringValue = modalTypeNames[newIndex];
                property.serializedObject.ApplyModifiedProperties();
            }

            position.y += lineHeight + padding;

            EditorGUI.PropertyField(position, assetRefProp);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return lineHeight * 3 + padding * 4;
        }
    }
}
#endif