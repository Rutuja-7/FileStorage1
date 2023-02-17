using Microsoft.AspNetCore.Mvc;
using FileStorage.Services;
using Azure.Storage.Blobs.Models;

namespace FileStorage.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class BlobController : ControllerBase
	{
		private readonly IBlobStorage _storage;
		private readonly string _connectionString ;
		private readonly string _container;

		public BlobController(IBlobStorage storage, IConfiguration iConfig)
		{
			_storage = storage;
			_connectionString = iConfig.GetValue<string>("MyConfig:ConnectionString");
			_container = iConfig.GetValue<string>("MyConfig:ContainerName");
		}

		/// <summary>
		/// Creates a new container 
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("CreateContainer")]
		public async Task<bool> CreateContainer()
		{
			var result = await _storage.CreateContainer(_connectionString, _container);
			return result;

		}

		/// <summary>
		/// Displays list of files available in container
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("ListFiles")]
		public async Task<List<string>> ListFiles()
		{
			var result = await _storage.GetAllDocuments(_connectionString, _container);
			return result;
		}


		/// <summary>
		/// Inserts a new file to particular container
		/// </summary>
		/// <param name="formFile"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("InsertFile")]
		public async Task<bool> InsertFile(IFormFile formFile)
		{
			if(formFile != null)
			{
				Stream stream = formFile.OpenReadStream();
				await _storage.UploadDocument(_connectionString, _container, formFile.FileName, stream);
				return true;
			}
			return false;
		}


		/// <summary>
		/// Download a particular file from the container
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("DownloadFile")]
		public async Task<IActionResult> DownloadFile(string fileName)
		{
			var content = await _storage.GetDocument(_connectionString, _container, fileName);
			return File(content, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
		}


		/// <summary>
		/// Deletes the particular file from the container 
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("DeleteFile")]
		public async Task<bool> DeleteFile(string fileName)
		{
			var result = await _storage.DeleteDocument(_connectionString, _container, fileName);
			return result;
		}


		/// <summary>
		/// Deletes a container
		/// </summary>
		/// <returns></returns>
        [HttpDelete]
        [Route("DeleteContainer")]
        public async Task<bool> DeleteContainer()
        {
            var result = await _storage.DeleteContainer(_connectionString, _container);
            return result;
        }
    }
}
