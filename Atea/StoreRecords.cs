using System.Threading.Tasks;
using Atea.Core.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Atea
{
    public class StoreRecords
    {
        private readonly IBlobService _blobService;

        private readonly ITableService _tableService;

        private readonly IHTTPService _httpService;

        public StoreRecords(IBlobService blobService, ITableService tableService, IHTTPService httpService)
        {
            _blobService = blobService;

            _tableService = tableService;

            _httpService = httpService;
        }

        [FunctionName("StoreRecords")]
        public async Task Run([TimerTrigger("*/10 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            var data = await _httpService.GetData();

            var success = data.IsSuccessStatusCode;

            var payload = await data.Content.ReadAsStringAsync();

            var blobId = await _blobService.StoreFile(payload);

            await _tableService.StoreRecord(success, blobId);

            log.LogInformation($"Records Created!");
        }
    }
}
