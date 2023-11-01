using Atea.Core.Configuration;
using Atea.Core.Services;
using Azure.Data.Tables;

namespace Atea.Services
{
    public class TableService : ITableService
    {
        private readonly TableClient _tableClient;

        public TableService(IStorageConfiguration storageConfiguration)
        {
            _tableClient = new TableClient(storageConfiguration.ConnectionString, storageConfiguration.StorageName);
            _tableClient.CreateIfNotExists();
        }

        public Record[] GetFilteredRecords(DateTime from, DateTime to)
        {
            var records = _tableClient.Query<Record>(record => record.Timestamp > from && record.Timestamp < to);

            return records.ToArray();
        }

        public async Task<Record?> GetSingleRecord(string id)
        {
            var record = await _tableClient.GetEntityIfExistsAsync<Record>(id, id);

            return record.Value;

        }

        public async Task StoreRecord(bool status, string blobName)
        {
            var id = Guid.NewGuid().ToString();

            var record = new Record
            {
                RowKey = id,
                PartitionKey = id,
                WasSuccessfull = status,
                BlobContainersID = blobName
            };

            await _tableClient.AddEntityAsync(record);
        }
    }
}
