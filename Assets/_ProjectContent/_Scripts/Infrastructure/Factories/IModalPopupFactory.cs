using Cysharp.Threading.Tasks;
using Infrastructure.Services.Modals;

namespace Infrastructure.Factories
{
    public interface IModalPopupFactory
    {
        UniTask<T> Show<T>() where T : ModalPopup;
    }
}