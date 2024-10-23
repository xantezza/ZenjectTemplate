using UnityEngine;

namespace Infrastructure.Services.OnGUIService
{
    public interface IOnGUIService
    {
        void AddDevOnGUIElement<T>(T element) where T : MonoBehaviour, IDevOnGUIElement;
    }
}