using Cysharp.Threading.Tasks;

namespace Infrastructure.Services.LoadingCurtain
{
    public interface ILoadingCurtainService
    {
        UniTask Show(float tweenDuration = 0.3f);
        void ForceShow();
        void SetProgress01(float value);
        void Hide(float tweenDuration = 0.3f);
        void ForceHide();
    }
}