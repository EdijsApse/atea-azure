using Atea.Core.Services;
using Azure.Storage.Blobs;
using System.IO;

namespace Atea.Services
{
    public class BlobService : IBlobService
    {
        public async Task<string> GetFileContent(string blobName)
        {
            var client = GetBlobClient(blobName).Result;

            var content = await client.DownloadContentAsync();

            return content.Value.Content.ToString();
        }

        public async Task<string> StoreFile(string payload)
        {
            var blobName = Guid.NewGuid().ToString();

            var client = GetBlobClient(blobName).Result;

            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                await streamWriter.WriteAsync(payload);
                await streamWriter.FlushAsync();

                memoryStream.Position = 0;

                await client.UploadAsync(memoryStream);
            }

            return blobName;
        }

        private async Task<BlobContainerClient> GetContainerClient()
        {
            var containerClient = new BlobContainerClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), Environment.GetEnvironmentVariable("StorageName"));

            if (!containerClient.Exists())
            {
                await containerClient.CreateIfNotExistsAsync();
            }

            return containerClient;
        }

        private async Task<BlobClient> GetBlobClient(string blobName)
        {
            var conatiner = await GetContainerClient();

            var client = conatiner.GetBlobClient(blobName);

            return client;
        }
    }
}
