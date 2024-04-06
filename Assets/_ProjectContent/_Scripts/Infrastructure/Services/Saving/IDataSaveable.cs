namespace Infrastructure.Services.Saving
{
    public interface IDataSaveable<TSave>
    {
        SaveKey SaveId { get; }

        TSave SaveData { get; set; }
    }
}