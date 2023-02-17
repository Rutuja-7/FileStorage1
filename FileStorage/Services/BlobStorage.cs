using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace FileStorage.Services
{
    public class BlobStorage : IBlobStorage
	{
		public async Task<bool> CreateContainer(string connectionString, string containerName)
		{
            BlobServiceClient blobServiceClient = new(connectionString);
            await blobServiceClient.CreateBlobContainerAsync(containerName);
            var container = blobServiceClient.GetBlobContainerClient(containerName);
            if (await container.ExistsAsync()) 
			{
				Console.WriteLine($"Container created successfully: {containerName}");
				return true;
			}
			else
			{
				return false;
			}
		}
		public async Task<List<string>> GetAllDocuments(string connectionString, string containerName)
		{
			var container = BlobExtensions.GetContainer(connectionString, containerName);
			if(!await container.ExistsAsync())
			{
				return new List<string>();
			}

			List<string> blobs = new();
			await foreach(BlobItem blobItem in container.GetBlobsAsync())
			{
				blobs.Add(blobItem.Name);
			}
			Console.WriteLine($"\nList of all blobs present in {containerName}: {blobs}");
			return blobs;
		}

		public async Task<Stream> GetDocument(string connectionString, string containerName, string fileName)
		{
			var container = BlobExtensions.GetContainer(connectionString, containerName);
			if(await container.ExistsAsync())
			{
				var blobClient = container.GetBlobClient(fileName);
				if(await blobClient.ExistsAsync())
				{
					var content = await blobClient.DownloadStreamingAsync();
					Console.WriteLine($"Download the {fileName} successfully.");
					return content.Value.Content;
				}
				else
				{
					throw new FileNotFoundException();
				}
			}
			else
			{
				throw new FileNotFoundException();
			}
		}

		public async Task<bool> DeleteDocument(string connectionString, string containerName, string fileName)
		{
			var container = BlobExtensions.GetContainer(connectionString, containerName);
			if(!await container.ExistsAsync())
			{
				return false;
			}

			var blobClient = container.GetBlobClient(fileName);
			if(await blobClient.ExistsAsync())
			{
				Console.WriteLine($"Delete the {fileName} successfully.");
				await blobClient.DeleteAsync();
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task UploadDocument(string connectionString, string containerName, string fileName, Stream fileContent)
		{
			var container = BlobExtensions.GetContainer(connectionString, containerName);
			if(!await container.ExistsAsync())
			{
				BlobServiceClient blobServiceClient = new(connectionString);
				await blobServiceClient.CreateBlobContainerAsync(containerName);
				container = blobServiceClient.GetBlobContainerClient(containerName);
			}

			var blobClient = container.GetBlobClient(fileName);
			if(!blobClient.Exists())
			{
				fileContent.Position = 0;
				Console.WriteLine($"Upload the {fileName} succesfully.");
				await container.UploadBlobAsync(fileName, fileContent);
			}
			else
			{
				fileContent.Position = 0;
                Console.WriteLine($"Upload the {fileName} succesfully.");
                await blobClient.UploadAsync(fileContent, overwrite: true);
			}
		}

		public async Task<bool> DeleteContainer(string connectionString, string containerName)
		{
			BlobServiceClient blobServiceClient = new(connectionString);
			if(blobServiceClient == null)
			{
				return false;

			}
			else
			{
				Console.WriteLine($"Deleted container successfully : {containerName}");
                await blobServiceClient.DeleteBlobContainerAsync(containerName);
				return true;
            }
			
		}
	}
	public static class BlobExtensions
	{
		public static BlobContainerClient GetContainer(string connectionString, string containerName)
		{
			BlobServiceClient blobServiceClient = new(connectionString);
			return blobServiceClient.GetBlobContainerClient(containerName); 
		}
	}

}
