using Cysharp.Threading.Tasks;

namespace Infrastructure.Factories.ModalPopup
{
    public interface IModalPopupFactory
    {
        UniTask<T> Show<T>() where T : Services.Modals.ModalPopup;
        UniTask<T> Create<T>() where T : Services.Modals.ModalPopup;
    }
}