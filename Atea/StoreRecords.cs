using System;
using System.Net.Http;
using System.Threading.Tasks;
using Atea.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Atea
{
    public class StoreRecords
    {
        [FunctionName("StoreRecords")]
        public async Task Run([TimerTrigger("*/10 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            var httpService = new HTTPService(new HttpClient());
            
            var blobService = new BlobService();

            var tableService = new TableService();

            var data = await httpService.GetData(Environment.GetEnvironmentVariable("APIEndpoint"));

            var success = data.IsSuccessStatusCode;

            var payload = await data.Content.ReadAsStringAsync();

            var blobId = await blobService.StoreFile(payload);

            await tableService.StoreRecord(success, blobId);

            log.LogInformation($"Records Created!");
        }
    }
}
