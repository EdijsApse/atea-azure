using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Atea.Services;
using Microsoft.AspNetCore.Http;

namespace Atea
{
    public static class GetSingleRecord
    {
        [FunctionName("GetSingleRecord")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "record/{id}")]HttpRequest req, string id)
        {
            var tableService = new TableService();

            var record = await tableService.GetSingleRecord(id);

            if (record == null)
            {
                return new NotFoundObjectResult(new {
                    message = "Record Not Found!"
                });
            }

            var blobService = new BlobService();

            var content = blobService.GetFileContent(record.BlobContainersID);

            return new OkObjectResult(content.Result);
        }
    }
}
