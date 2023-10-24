using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System;
using Atea.Services;
using Atea.Core.Validators;

namespace Atea
{
    public static class GetRecords
    {
        [FunctionName("GetRecords")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "records")] HttpRequest req)
        {
            var dateFromString = req.Query["date-from"];
            var dateToString = req.Query["date-to"];

            var dateValidator = new DateValidator(dateFromString, dateToString);

            if (!dateValidator.IsValid())
            {
                return new OkObjectResult(new
                {
                    success = false,
                    message = dateValidator.ErrorMessage
                });
            }

            var dateFrom = DateTime.Parse(dateFromString);
            var dateTo = DateTime.Parse(dateToString);

            var tableService = new TableService();

            var records = await tableService.GetFilteredRecords(dateFrom, dateTo);

            return new OkObjectResult(new {
                data = records
            });
        }
    }
}
