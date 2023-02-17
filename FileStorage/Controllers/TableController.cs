using Microsoft.AspNetCore.Mvc;
using FileStorage.Services;
using FileStorage.Models;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;

namespace FileStorage.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TableController : ControllerBase
	{
		private readonly ITableStorage _tableStorage;
		private readonly string _connectionString;
		private readonly string _tableName;

		public TableController(ITableStorage storage, IConfiguration iConfig)
		{
			_tableStorage = storage;
			_connectionString = iConfig.GetValue<string>("MyConfig:ConnectionString");
			_tableName = iConfig.GetValue<string>("MyConfig:TableName");
		}


		/// <summary>
		/// Create new table
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("CreateTable")]
		public async Task CreateTable()
		{
			await _tableStorage.CreateTable(_connectionString, _tableName);
		}


		/// <summary>
		/// Get employee entity using department and id
		/// </summary>
		/// <param name="department"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("GetEntity")]
		[ActionName(nameof(GetEmployeeAsync))]
		public async Task<EmployeeEntity> GetEmployeeAsync([FromQuery] string department, string id)
		{
			var result = await _tableStorage.GetEmployeeEntity(_connectionString, _tableName, department, id);
			return result;
		}


		/// <summary>
		/// Insert a new employee
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("InsertEntity")]
		public async Task<IActionResult> InsertEmployeeAsync([FromBody] EmployeeDTO empdto)
		{
			EmployeeEntity employeeEntity = new EmployeeEntity()
			{
				Department = empdto.Department,
				Id = empdto.Id,
				Name = empdto.Name,
				Salary = empdto.Salary
			};
            employeeEntity.PartitionKey = empdto.Department;
            employeeEntity.RowKey = empdto.Id;
			var createdEntity = await _tableStorage.AddEmployeeEntity(_connectionString, _tableName, employeeEntity);
			return CreatedAtAction(nameof(GetEmployeeAsync), createdEntity);
		}



		/// <summary>
		/// Update the existing employee
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("UpdateEntity")]
		public async Task<IActionResult> UpdateEmployeeAsync([FromBody] EmployeeDTO employeeDTO)
		{
            EmployeeEntity employeeEntity = new EmployeeEntity()
            {
                Department = employeeDTO.Department,
                Id = employeeDTO.Id,
                Name = employeeDTO.Name,
                Salary = employeeDTO.Salary
            };
            employeeEntity.PartitionKey = employeeDTO.Department;
            employeeEntity.RowKey = employeeDTO.Id;
			await _tableStorage.UpsertEmployeeEntity(_connectionString, _tableName, employeeEntity);
			return NoContent();
		}


		/// <summary>
		/// Delete the employee entity
		/// </summary>
		/// <param name="category"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("DeleteEntity")]
		public async Task DeleteEmployeeAsync([FromQuery] string category, string id)
		{
			await _tableStorage.DeleteEmployeeEntity(_connectionString, _tableName, category, id);
		}


		/// <summary>
		/// Delete the table
		/// </summary>
		/// <returns></returns>
		[HttpDelete]
		[Route("DeleteTable")]
		public async Task DeleteTable()
		{
			await _tableStorage.DeleteTable(_connectionString, _tableName);
		}
	}
}
