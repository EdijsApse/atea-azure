using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using System;

namespace Atea
{
    public static class GetSingleRecord
    {
        [FunctionName("GetSingleRecord")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "record/{id}")] HttpRequest req, string id, ILogger log)
        {
            var tableServiceClient = new TableServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            var tableClient = tableServiceClient.GetTableClient(Environment.GetEnvironmentVariable("StorageName"));
            var record = await tableClient.GetEntityIfExistsAsync<Record>(id, id);

            if (!record.HasValue)
            {
                return new NotFoundObjectResult(new {
                    message = "Record Not Found!"
                });
            }

            var blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("StorageName"));

            var blob = containerClient.GetBlobClient($"{record.Value.BlobContainersID}");
            var blobContent = await blob.DownloadContentAsync();

            return new OkObjectResult(blobContent.Value.Content.ToString());
        }
    }
}
