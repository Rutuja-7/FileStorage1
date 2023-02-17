using FileStorage.Models;

namespace FileStorage.Services
{
    public interface IFileShareStorage
    {
        public Task<bool> CreateDirectory(string connectionString, string fileShareName, string directoryName);
        public Task UploadFile(string connectionString, string fileShareName, string directoryName, IFormFile formFile);
        public Task<byte[]> DownloadFile(string connectionString, string fileShareName,  string directoryName, string fileName);
        public Task<bool> DeleteDirectory(string connectionString, string fileShareName, string directoryName);
        public Task DeleteFile(string connectionString, string fileShareName, string directoryName, string fileName);
    }
}
