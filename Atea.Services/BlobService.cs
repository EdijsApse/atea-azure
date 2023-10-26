using Atea.Core.Configuration;
using Atea.Core.Services;
using Azure.Storage.Blobs;

namespace Atea.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(IStorageConfiguration storageConfiguration)
        {
            _containerClient = new BlobContainerClient(storageConfiguration.ConnectionString, storageConfiguration.StorageName);

            _containerClient.CreateIfNotExists();
        }

        public async Task<string> GetFileContent(string blobName)
        {
            var client = GetBlobClient(blobName);

            var content = await client.DownloadContentAsync();

            return content.Value.Content.ToString();
        }

        public async Task<string> StoreFile(string payload)
        {
            var blobName = Guid.NewGuid().ToString();

            var client = GetBlobClient(blobName);

            var stream = await WritePayloadToStream(payload);

            await client.UploadAsync(stream);

            return blobName;
        }

        private async Task<MemoryStream> WritePayloadToStream(string payload)
        {
            var memoryStream = new MemoryStream();

            var streamWriter = new StreamWriter(memoryStream);
            await streamWriter.WriteAsync(payload);
            await streamWriter.FlushAsync();

            memoryStream.Position = 0;

            return memoryStream;
        }

        private BlobClient GetBlobClient(string blobName)
        {
            var client = _containerClient.GetBlobClient(blobName);

            return client;
        }
    }
}
