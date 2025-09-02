using Cysharp.Threading.Tasks;
using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Services.Saving;
using Infrastructure.Services.SceneLoading;
using Zenject;

namespace Infrastructure.StateMachines.GameLoopStateMachine.States
{
    public class GameplayState : BaseGameLoopState, IEnterableState
    {
        private readonly ISaveService _saveService;
        private readonly ISceneLoaderService _sceneLoaderService;
        private readonly IAssetReferenceProvider _assetReferenceProvider;

        [Inject]
        public GameplayState(
            GameLoopStateMachine gameLoopStateMachine, 
            ISaveService saveService,
            ISceneLoaderService sceneLoaderService, 
            IAssetReferenceProvider assetReferenceProvider
            ) : base(gameLoopStateMachine)
        {
            _assetReferenceProvider = assetReferenceProvider;
            _sceneLoaderService = sceneLoaderService;
            _saveService = saveService;
        }

        public async UniTask Enter()
        {
            await _sceneLoaderService.LoadScene(_assetReferenceProvider.GamePlayScene);
        }

        public override UniTask Exit()
        {
            _saveService.StoreSaveFile();
            return default;
        }
    }
}