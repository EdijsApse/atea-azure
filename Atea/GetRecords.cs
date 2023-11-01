using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System;
using Atea.Core.Validators;
using Atea.Core.Services;

namespace Atea
{
    public class GetRecords
    {
        private readonly IDateValidator _dateValidator;

        private readonly ITableService _tableService;

        public GetRecords(IDateValidator dateValidator, ITableService tableService)
        {
            _dateValidator = dateValidator;
            _tableService = tableService;
        }

        [FunctionName("GetRecords")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "records")] HttpRequest req)
        {
            var dateFromString = req.Query["date-from"];
            var dateToString = req.Query["date-to"];

            if (!_dateValidator.IsValid(dateFromString, dateToString))
            {
                return new OkObjectResult(new
                {
                    success = false,
                    message = _dateValidator.ErrorMessage
                });
            }

            var dateFrom = DateTime.Parse(dateFromString);
            var dateTo = DateTime.Parse(dateToString);

            var records = _tableService.GetFilteredRecords(dateFrom, dateTo);

            return new OkObjectResult(new {
                data = records
            });
        }
    }
}
