namespace Infrastructure.Services.Saving
{
    public interface ISaveService
    {
        void Process<TSave>(IDataSaveable<TSave> dataSaveable) where TSave : class, new();

        void LoadSaveFile(bool useDefaultFileName = true, string fileName = null);
        void StoreSaveFile(bool useDefaultFileName = true, string fileName = null);
    }
}