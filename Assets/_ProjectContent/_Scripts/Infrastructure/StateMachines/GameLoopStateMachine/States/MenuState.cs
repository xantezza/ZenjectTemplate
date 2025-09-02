using Cysharp.Threading.Tasks;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Services.SceneLoading;
using Infrastructure.StateMachines.StateMachine;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class MenuState : BaseGameLoopState, IEnterableState
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IAssetReferenceProvider _assetReferenceProvider;

        [Inject]
        public MenuState(
            GameLoopStateMachine stateMachine,
            ISceneLoaderService sceneLoaderService,
            IAssetReferenceProvider assetReferenceProvider
            ) : base(stateMachine)
        {
            _assetReferenceProvider = assetReferenceProvider;
            _sceneLoaderService = sceneLoaderService;
        }

        public async UniTask Enter()
        {
            await _sceneLoaderService.LoadScene(_assetReferenceProvider.MenuScene);
        }
    }
}