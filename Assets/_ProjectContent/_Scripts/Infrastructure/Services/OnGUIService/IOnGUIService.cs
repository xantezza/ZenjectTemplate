using UnityEngine;

namespace Infrastructure.Services.DevGUIService
{
    public interface IOnGUIService
    {
        void AddDevOnGUIElement<T>(T element) where T : MonoBehaviour, IDevOnGUIElement;
    }
}