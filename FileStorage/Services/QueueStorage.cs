using System;
using Azure.Core;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace FileStorage.Services
{
	public class QueueStorage : IQueueStorage
	{
		public async Task<bool> CreateQueue(string connectionString, string queueName)
		{
			try
			{
				//Instantiate the queueClient which will be used to create and manipulate the queue.
				QueueClient queueClient = new QueueClient(connectionString, queueName);

				//create the queue.
				await queueClient.CreateIfNotExistsAsync();

				if (queueClient.Exists())
				{
					Console.WriteLine($"Queue Created Successfully : '{queueClient.Name}'");
					return true;
				}
				else
				{
					Console.WriteLine("Error occured while creating queue.\n");
					return false;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"\nException: {ex.Message}");
				return false;
			}

		}
		public async Task InsertMessage(string connectionString, string queueName, string message)
		{
			QueueClient queueClient = new QueueClient(connectionString, queueName);
			await queueClient.CreateIfNotExistsAsync();

			if (queueClient.Exists())
			{
				await queueClient.SendMessageAsync(message);
			}
			Console.WriteLine($"Inserted Message: {message}");
		}

		public async Task UpdateMessage(string connectionString, string queueName, string message)
		{
			QueueClient queueClient = new QueueClient(connectionString, queueName);

			if (queueClient.Exists())
			{
				QueueMessage[] messages = await queueClient.ReceiveMessagesAsync();

				await queueClient.UpdateMessageAsync(messages[0].MessageId, messages[0].PopReceipt, message);
			}
			Console.WriteLine($"Updated message: {message}");
		}
		public async Task ReadMessage(string connectionString, string queueName)
		{
			QueueClient queueClient = new QueueClient(connectionString, queueName);
			if (!await queueClient.ExistsAsync())
			{
				throw new Exception();
			}
			QueueMessage[] retrivedMessages = await queueClient.ReceiveMessagesAsync(1);
			string message = retrivedMessages[0].MessageText;
			Console.WriteLine($"Readed Message: {message}");
		}

		public async Task<bool> DeleteMessage(string connectionString, string queueName)
		{
			QueueClient queueClient = new QueueClient(connectionString, queueName);
			if (queueClient.Exists())
			{
				QueueMessage[] retrivedMessage = await queueClient.ReceiveMessagesAsync();
				Console.WriteLine($"Deleted Message: '{retrivedMessage[0].Body}'");
				await queueClient.DeleteMessageAsync(retrivedMessage[0].MessageId, retrivedMessage[0].PopReceipt);
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task<bool> DeleteQueue(string connectionString, string queueName)
		{
			QueueClient queueClient = new QueueClient(connectionString, queueName);
			if (await queueClient.ExistsAsync())
			{
				Console.WriteLine($"\nDeleted queue: {queueClient.Name}");
				await queueClient.DeleteAsync();
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
