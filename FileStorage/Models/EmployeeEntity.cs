using Azure;
using Azure.Data.Tables;

namespace FileStorage.Models
{
	public class EmployeeEntity : ITableEntity
	{
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public int Salary { get; set; }
        public string PartitionKey { get; set; }
		public string RowKey { get; set; }
		public DateTimeOffset? Timestamp { get; set; }

		public ETag ETag { get; set; }
	}
}
