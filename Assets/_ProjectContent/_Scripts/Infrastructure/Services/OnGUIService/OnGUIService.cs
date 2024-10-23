using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace Infrastructure.Services.OnGUIService
{
    public class OnGUIService : MonoBehaviour, IOnGUIService
    {
        private const float _messageLifetime = 6f;

        private bool _guiEnabled;
        private int _index;

        // MonoBehaviour list to take advantage of Unity's ability to replace destroyed objects with nulls and not bother with removing elements from outside
        private readonly List<MonoBehaviour> _elements = new();

        private readonly List<Message> _messages = new();

        private class Message
        {
            public Message(string messageText, float lifeTime)
            {
                MessageText = messageText;
                LifeTime = lifeTime;
            }

            public string MessageText;
            public float LifeTime;
        }

        public void AddDevOnGUIElement<T>(T element) where T : MonoBehaviour, IDevOnGUIElement
        {
#if DEV
            _elements.Add(element);
#endif
        }

        [Button]
        public void ShowMessage(string textToShowUp)
        {
            _messages.Add(new Message(textToShowUp, _messageLifetime));
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
#if DEV
            DevOnGUI();
#endif

            for (var i = _messages.Count - 1; i >= 0; i--)
            {
                var message = _messages[i];
                message.LifeTime -= Time.unscaledDeltaTime;
                if (message.LifeTime < 0) _messages.RemoveAt(i);
                GUILayout.Label(message.MessageText);
            }
            
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
                    if (_elements[i] == null)
                    {
                        _elements.RemoveAt(i);
                    }
                }

                if (_elements.Count > 0)
                {
                    GUILayout.Label($"Element {_index + 1}/{_elements.Count}: {_elements[_index]}");

                    if (GUILayout.Button("Next"))
                    {
                        _index++;
                    }

                    if (_index < 0) _index = _elements.Count - 1;
                    if (_index >= _elements.Count) _index = 0;

                    ((IDevOnGUIElement) _elements[_index]).DrawDevGUI();
                }
            }
        }
    }
}