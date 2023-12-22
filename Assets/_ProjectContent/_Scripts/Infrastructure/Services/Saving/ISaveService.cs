namespace Infrastructure.Services.Saving
{
    public interface ISaveService
    {
        void AddToSave<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class;

        void Load<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class;

        void LoadSaveFile(bool useDefaultFileName = true, string fileName = null);
        void StoreSaveFile(bool useDefaultFileName = true, string fileName = null);
    }
}