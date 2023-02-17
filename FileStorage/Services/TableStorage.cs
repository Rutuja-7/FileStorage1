using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using FileStorage.Models;
using FileStorage.Services;

namespace FileStorage.Services
{
	public class TableStorage : ITableStorage
	{

		private async Task<TableClient> GetTableClient(string connectionString, string tableName)
		{
			TableServiceClient client = new TableServiceClient(connectionString);

			var tableClient = client.GetTableClient(tableName);
			await tableClient.CreateIfNotExistsAsync();
			Console.WriteLine("Getting table client..");
			return tableClient;
		}

		public async Task<TableItem> CreateTable(string connectionString, string tableName)
		{
			TableServiceClient client = new TableServiceClient(connectionString);
			var table = await client.CreateTableAsync(tableName);

			Console.WriteLine($"Table created successfully: {tableName}");
			return table;
		}

		public async Task<EmployeeEntity> GetEmployeeEntity(string connectionString, string tableName, string department, string id)
		{
			var tableClient = await GetTableClient(connectionString, tableName);
			var result = await tableClient.GetEntityAsync<EmployeeEntity>(department, id);
			Console.WriteLine($"\nGetting employee information with department = {department} and id = {id}");
			Console.WriteLine($"Employee information : {result}");
            return result;
		}

		public async Task<EmployeeEntity> AddEmployeeEntity(string connectionString, string tableName, EmployeeEntity employeeEntity)
		{
			var tableClient = await GetTableClient(connectionString, tableName);
			await tableClient.AddEntityAsync(employeeEntity);
			return employeeEntity;
		}

        public async Task<EmployeeEntity> UpsertEmployeeEntity(string connectionString, string tableName, EmployeeEntity employeeEntity)
		{
			var tableClient = await GetTableClient(connectionString, tableName);
			await tableClient.UpsertEntityAsync(employeeEntity);
			return employeeEntity;
		}

		public async Task DeleteEmployeeEntity(string connectionString, string tableName, string department, string id)
		{
			var tableClient = await GetTableClient(connectionString, tableName);
			await tableClient.DeleteEntityAsync(department, id);
			Console.WriteLine("Deleted epmloyee successfully");
		}

		public async Task DeleteTable(string connectionString, string tableName)
		{
			TableServiceClient client = new TableServiceClient(connectionString);
			await client.DeleteTableAsync(tableName);
			Console.WriteLine($"Table deleted successfully: {tableName}");
		}
	}
}
