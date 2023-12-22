namespace Infrastructure.Services.Saving
{
    public interface IDataSaveable<TSave>
    {
        string SaveId();

        TSave SaveData { get; set; }
        
        TSave Default { get; }
    }
}