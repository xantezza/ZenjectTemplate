using UnityEngine;

namespace Infrastructure.Services.OnGuiDev
{
    public interface IDevGUIService
    {
        void Add<T>(T element) where T : MonoBehaviour, IDevGUIElement;
    }
}