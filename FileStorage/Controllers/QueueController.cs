using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Queues.Models;
using FileStorage.Services;

namespace FileStorage.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class QueueController : ControllerBase
	{
		private readonly IQueueStorage _queueStorage;
		private readonly string _connectionString;
		private readonly string _queueName;

		public QueueController(IQueueStorage queueStorage, IConfiguration iConfig)
		{
			_queueStorage = queueStorage;
			_connectionString = iConfig.GetValue<string>("MyConfig:ConnectionString");
			_queueName = iConfig.GetValue<string>("MyConfig:QueueName");
		}


		/// <summary>
		/// Create a new queue if it is not exists
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("CreateQueue")]
		public async Task<bool> CreateQueue()
		{
			var result = await _queueStorage.CreateQueue(_connectionString, _queueName);
			return result;
		}

		/// <summary>
		/// Create and Insert the message in a queue if it is not exists
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("InsertMessage")]
		public async Task InsertMessage(string message)
		{
            if (message != null)
                await _queueStorage.InsertMessage(_connectionString, _queueName, message);
		}

		/// <summary>
		/// Update a message
		/// </summary>
		/// <returns></returns>
		[HttpPut]
		[Route("UpdateMessage")]
		public async Task UpdateMessage(string message)
		{
			if(message!=null)
				await _queueStorage.UpdateMessage(_connectionString, _queueName, message);
		}

		/// <summary>
		/// Read the message from queue
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("ReadMessage")]
		public async Task ReadMessage()
		{
			await _queueStorage.ReadMessage(_connectionString, _queueName);
		}

		/// <summary>
		/// Delete the message from queue
		/// </summary>
		/// <returns></returns>
		[HttpDelete]
		[Route("DeleteMessage")]
		public async Task<bool> DeleteMessage()
		{
			var result = await _queueStorage.DeleteMessage(_connectionString, _queueName);
			return result;
		}

		/// <summary>
		/// Delete the queue if it exists 
		/// </summary>
		/// <returns></returns>
		[HttpDelete]
		[Route("DeleteQueue")]
		public async Task<bool> DeleteQueue()
		{
			var result = await _queueStorage.DeleteQueue(_connectionString, _queueName);
			return result;
		}
	}
}
