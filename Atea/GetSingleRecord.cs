using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Atea.Core.Services;

namespace Atea
{
    public class GetSingleRecord
    {

        private readonly ITableService _tableService;

        private readonly IBlobService _blobService;

        public GetSingleRecord(IBlobService blobService, ITableService tableService)
        {
            _tableService = tableService;
            _blobService = blobService;
        }

        [FunctionName("GetSingleRecord")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "record/{id}")]HttpRequest req, string id)
        {
            var record = await _tableService.GetSingleRecord(id);

            if (record == null)
            {
                return new NotFoundObjectResult(new {
                    message = "Record Not Found!"
                });
            }

            var content = _blobService.GetFileContent(record.BlobContainersID);

            return new OkObjectResult(content.Result);
        }
    }
}
