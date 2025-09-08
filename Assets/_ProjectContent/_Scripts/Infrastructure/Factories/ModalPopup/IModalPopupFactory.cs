using Cysharp.Threading.Tasks;

namespace Infrastructure.Factories.ModalPopup
{
    public interface IModalPopupFactory
    {
        UniTask<T> Show<T>() where T : Services.Modals.ModalPopup;
        UniTask<T> Spawn<T>() where T : Services.Modals.ModalPopup;
        void ReturnToPool<T>(T modalPopup) where T : Services.Modals.ModalPopup;
    }
}