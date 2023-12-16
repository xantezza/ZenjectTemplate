namespace Infrastructure.Services.Saving
{
    public interface IDataSaveable<TSave>
    {
        string GetDataSaveId();

        TSave SaveData { get; set; }
    }
}