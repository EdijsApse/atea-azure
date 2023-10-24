namespace Atea.Core.Services
{
    public interface ITableService
    {
        Task StoreRecord(bool status, string blobName);

        Task<Record[]> GetFilteredRecords(DateTime from, DateTime to);

        Task<Record?> GetSingleRecord(string id);
    }
}
