using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace Infrastructure.Services.Saving
{
    public interface ISaveService
    {
        [CanBeNull] TSave Load<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class;
        void AddToSaveables<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class;

        void LoadSaveFile(bool useDefaultFileName = true, string fileName = null);
        UniTask StoreSaveFile(bool useDefaultFileName = true, string fileName = null);
    }
}