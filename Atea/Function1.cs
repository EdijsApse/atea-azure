using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Atea
{
    public class Function1
    {
        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("*/10 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            var http = new HttpClient();

            var data = await http.GetAsync(Environment.GetEnvironmentVariable("APIEndpoint"));

            var success = data.IsSuccessStatusCode;
            var payload = await data.Content.ReadAsStringAsync();

            var blobId = CreateBlobRecord(payload);

            CreateTableRecord(success, blobId.Result);

            log.LogInformation($"Records Created!");
        }

        public async Task<string> CreateBlobRecord(string responsePayload)
        {
            var blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            var nameOfFile = Guid.NewGuid().ToString();

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("StorageName"));

            if (!containerClient.Exists())
            {
                containerClient = await blobServiceClient.CreateBlobContainerAsync(Environment.GetEnvironmentVariable("StorageName"));
            }

            string localPath = "data";
            Directory.CreateDirectory(localPath);

            string fullFileName = nameOfFile;
            string localFilePath = Path.Combine(localPath, fullFileName);

            await File.WriteAllTextAsync(localFilePath, responsePayload);

            BlobClient blobClient = containerClient.GetBlobClient(fullFileName);

            await blobClient.UploadAsync(localFilePath, true);

            return nameOfFile;
        }

        public async void CreateTableRecord(bool requestWasSuccessfull, string blobContainersName)
        {
            var tableServiceClient = new TableServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));

            var tableClient = tableServiceClient.GetTableClient(Environment.GetEnvironmentVariable("StorageName"));

            await tableClient.CreateIfNotExistsAsync();

            var id = Guid.NewGuid().ToString();

            var record = new Record
            {
                RowKey = id,
                PartitionKey = id,
                WasSuccessfull = requestWasSuccessfull,
                BlobContainersID = blobContainersName
            };

            await tableClient.AddEntityAsync(record);
        }
    }
}
