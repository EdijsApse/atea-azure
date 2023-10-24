using Atea.Core.Services;
using Azure.Data.Tables;

namespace Atea.Services
{
    public class TableService : ITableService
    {
        public async Task<Record[]> GetFilteredRecords(DateTime from, DateTime to)
        {
            var client = await GetTableClient();

            var records = client.Query<Record>(record => record.Timestamp > from && record.Timestamp < to);

            return records.ToArray();
        }

        public async Task<Record?> GetSingleRecord(string id)
        {
            var client = await GetTableClient();

            var record = await client.GetEntityIfExistsAsync<Record>(id, id);

            return record.Value;

        }

        public async Task StoreRecord(bool status, string blobName)
        {
            var client = await GetTableClient();

            var id = Guid.NewGuid().ToString();

            var record = new Record
            {
                RowKey = id,
                PartitionKey = id,
                WasSuccessfull = status,
                BlobContainersID = blobName
            };

            await client.AddEntityAsync(record);
        }

        private async Task<TableClient> GetTableClient()
        {
            var client = new TableClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), Environment.GetEnvironmentVariable("StorageName"));
            
            await client.CreateIfNotExistsAsync();

            return client;
        }
    }
}
