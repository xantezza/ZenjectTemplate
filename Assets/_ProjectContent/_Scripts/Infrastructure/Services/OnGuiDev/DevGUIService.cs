using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.OnGuiDev
{
    public class DevGUIService : MonoBehaviour, IDevGUIService
    {
        private bool _guiEnabled;
        private int _index;
        private readonly List<MonoBehaviour> _elements = new();

        public void Add<T>(T element) where T : MonoBehaviour, IDevGUIElement
        {
#if DEV
            _elements.Add(element);
#endif
        }

#if DEV
        private void OnGUI()
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
                GUILayout.BeginArea(new Rect(100, 0, 300, 1080));

                if (GUILayout.Button($"Enable Dev GUI: {_guiEnabled}"))
                {
                    _guiEnabled = !_guiEnabled;
                }

                if (_guiEnabled)
                {
                    GUILayout.Label($"Elements: {_elements.Count}");
                    GUILayout.Label($"Current Element: {_index + 1}");
                    if (GUILayout.Button("Prev"))
                    {
                        _index--;
                    }

                    if (GUILayout.Button("Next"))
                    {
                        _index++;
                    }

                    if (_index < 0) _index = _elements.Count - 1;
                    if (_index >= _elements.Count) _index = 0;

                    GUILayout.Label($"{_elements[_index]}");
                    ((IDevGUIElement) _elements[_index]).DrawDevGUI();
                }

                GUILayout.EndArea();
            }
        }
#endif
    }
}