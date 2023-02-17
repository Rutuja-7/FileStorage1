using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using FileStorage.Models;

namespace FileStorage.Services
{
	public interface ITableStorage
	{
		public Task<TableItem> CreateTable(string connectionString, string tableName);
		public Task<EmployeeEntity> GetEmployeeEntity(string connectionString, string tableName, string department, string id);
		public Task<EmployeeEntity> UpsertEmployeeEntity(string connectionString, string tableName, EmployeeEntity employeeEntity);
		public Task<EmployeeEntity> AddEmployeeEntity(string connectionString, string tableName, EmployeeEntity employeeEntity);
		public Task DeleteEmployeeEntity(string connectionString, string tableName, string department, string id);
		public Task DeleteTable(string connectionString, string tableName);
	}
}
