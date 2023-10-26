namespace Atea.Core.Configuration
{
    public interface IStorageConfiguration
    {
        string StorageName { get; }

        string ConnectionString { get; }
    }
}
