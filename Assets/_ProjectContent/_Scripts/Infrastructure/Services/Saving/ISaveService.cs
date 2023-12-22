namespace Infrastructure.Services.Saving
{
    public interface ISaveService
    {
        void AddToSave<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class;

        void Load<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class;

        void LoadAllData(bool useDefaultFileName = true, string fileName = null);
        void StoreAllSaveData(bool useDefaultFileName = true, string fileName = null);
    }
}