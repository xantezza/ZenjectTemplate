using UnityEngine;

namespace Editor.Utils
{
    public class VersionProvider : ScriptableObject
    {
        [field: SerializeField] public int Major { get; private set; }
        public int Minor;
    }
}