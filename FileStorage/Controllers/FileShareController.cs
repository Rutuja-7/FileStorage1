using Microsoft.AspNetCore.Mvc;
using FileStorage.Models;
using FileStorage.Services;

namespace FileStorage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileShareController : ControllerBase
    {
        private readonly IFileShareStorage _fileShareStorage;
        private readonly string _fileShareName;
        private readonly string _connectionString;
        private readonly string _directoryName;

        public FileShareController(IFileShareStorage fileShareStorage, IConfiguration iConfig)
        {
            _fileShareStorage = fileShareStorage;
            _connectionString = iConfig.GetValue<string>("MyConfig:ConnectionString");
            _fileShareName = iConfig.GetValue<string>("MyConfig:FileShareName");
            _directoryName = iConfig.GetValue<string>("MyConfig:DirectoryName");
        }


        /// <summary>
        /// Creates the directory
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateDirectory")]
        public async Task CreateDirectory()
        {
            await _fileShareStorage.CreateDirectory(_connectionString, _fileShareName, _directoryName);
        }
        /// <summary>
        /// Uploads the file
        /// </summary>
        /// <param name="fileDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] FileDetails fileDetails)
        {
            if(fileDetails.FileDetail != null)
            {
                await _fileShareStorage.UploadFile(_connectionString, _fileShareName, _directoryName, fileDetails.FileDetail);
            }
            return Ok();
        }

        /// <summary>
        /// Download the file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var result =  await _fileShareStorage.DownloadFile(_connectionString, _fileShareName, _directoryName, fileName);            
            if (result == null)
            {
                return NotFound();
            }
            return File(result, "application/octet-stream", fileName);
        }


        [HttpDelete]
        [Route("DeleteFile")]
        public async Task DeleteFile(string fileName)
        {
            await _fileShareStorage.DeleteFile(_connectionString, _fileShareName, _directoryName, fileName);
        }
        [HttpDelete]
        [Route("DeleteDirectory")]
        public async Task DeleteDirectory()
        {
            await _fileShareStorage.DeleteDirectory(_connectionString, _fileShareName, _directoryName);
        }
    }
}
