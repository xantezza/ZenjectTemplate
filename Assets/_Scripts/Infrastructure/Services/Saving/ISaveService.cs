using System;

namespace Infrastructure.Services.Saving
{
    public interface ISaveService : IDisposable
    {
        bool Save<TSave>(IDataSaveable<TSave> dataSaveable, bool overrideIfExist = true) where TSave : class;

        bool Load<TSave>(IDataSaveable<TSave> dataSaveable, bool createIfNotExist = true) where TSave : class;

        void DeleteAllData();
        void StoreSaveData();
    }
}