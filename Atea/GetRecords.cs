using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;
using System;
using System.Linq;

namespace Atea
{
    public static class GetRecords
    {
        [FunctionName("GetRecords")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "records")] HttpRequest req, ILogger log)
        {
            var dateFromString = req.Query["date-from"];
            var dateToString = req.Query["date-to"];

            if (!Validator.IsValidString(dateFromString) || !Validator.IsValidString(dateToString))
            {
                return new OkObjectResult(new
                {
                    success = false,
                    message = "date-from and date-to parameters should be provided!"
                });
            }

            if (!Validator.IsValidDateTime(dateFromString) || !Validator.IsValidDateTime(dateToString))
            {
                return new OkObjectResult(new {
                    success = false,
                    message = "Date format is invalid. Valid format yyyy-mm-dd!"
                });
            }

            var dateFrom = DateTime.Parse(dateFromString);
            var dateTo = DateTime.Parse(dateToString);

            if (!Validator.DateIntervalIsValid(dateFrom, dateTo))
            {
                return new OkObjectResult(new
                {
                    success = false,
                    message = "Date interval is not valid!"
                });
            }

            var tableServiceClient = new TableServiceClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));

            var tableClient = tableServiceClient.GetTableClient(Environment.GetEnvironmentVariable("StorageName"));

            var result = tableClient.Query<Record>(record => record.Timestamp > dateFrom && record.Timestamp < dateTo);

            return new OkObjectResult(new {
                data = result.ToArray()
            });
        }
    }
}
