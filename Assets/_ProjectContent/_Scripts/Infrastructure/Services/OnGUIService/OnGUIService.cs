using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.DevGUIService
{
    public class OnGUIService : MonoBehaviour, IOnGUIService
    {
        private bool _guiEnabled;
        private int _index;

        // MonoBehaviour list to take advantage of Unity's ability to replace destroyed objects with nulls and not bother with removing elements from outside
        private readonly List<MonoBehaviour> _elements = new();

        public void AddDevOnGUIElement<T>(T element) where T : MonoBehaviour, IDevOnGUIElement
        {
#if DEV
            _elements.Add(element);
#endif
        }

        public void ShowMessage(string textToShowUp)
        {
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
#if DEV
            DevOnGUI();
#endif
            
            
            Debug.LogError(Time.frameCount);
            
            GUILayout.EndVertical();
        }

        private void DevOnGUI()
        {
            if (GUILayout.Button($"DevOnGUI enabled: {_guiEnabled}"))
            {
                _guiEnabled = !_guiEnabled;
            }

            if (_guiEnabled)
            {
                for (var i = _elements.Count - 1; i >= 0; i--)
                {
                    if (_elements[_index] == null)
                    {
                        _elements.RemoveAt(_index);
                    }
                }

                if (_elements.Count > 0)
                {
                    GUILayout.Label($"Element {_index + 1}/{_elements.Count}");

                    if (GUILayout.Button("Next"))
                    {
                        _index++;
                    }

                    if (_index < 0) _index = _elements.Count - 1;
                    if (_index >= _elements.Count) _index = 0;

                    GUILayout.Label($"{_elements[_index]}");
                    ((IDevOnGUIElement) _elements[_index]).DrawDevGUI();
                }
            }

        }
    }
}