namespace Atea.Core.Services
{
    public interface IBlobService
    {
        Task<string> StoreFile(string payload);

        Task<string> GetFileContent(string blobName);
    }
}
