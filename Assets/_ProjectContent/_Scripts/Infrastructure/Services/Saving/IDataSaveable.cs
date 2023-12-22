namespace Infrastructure.Services.Saving
{
    public interface IDataSaveable<TSave>
    {
        string SaveId { get; }

        TSave SaveData { get; set; }
        
        TSave Default { get; }
    }
}