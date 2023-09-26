using Azure;
using Azure.Data.Tables;
using System;

namespace Atea
{
    public class Record : ITableEntity
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }

        public bool WasSuccessfull { get; set;}

        public string BlobContainersID { get; set; }
    }
}
