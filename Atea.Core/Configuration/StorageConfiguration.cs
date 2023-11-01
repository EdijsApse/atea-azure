namespace Atea.Core.Configuration
{
    public class StorageConfiguration : IStorageConfiguration
    {
        public string ConnectionString { get; }

        public string StorageName { get; }

        public StorageConfiguration()
        {
            ConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            StorageName = Environment.GetEnvironmentVariable("StorageName");
        }
    }
}
