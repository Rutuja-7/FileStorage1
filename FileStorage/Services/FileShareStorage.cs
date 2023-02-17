using Azure;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using FileStorage.Models;
using System.ComponentModel;
using System.IO;

namespace FileStorage.Services
{
    public class FileShareStorage : IFileShareStorage
    {
        public async Task<bool> CreateDirectory(string connectionString, string fileShareName, string directoryName)
        {
            ShareClient share = new ShareClient(connectionString, fileShareName);
            await share.CreateIfNotExistsAsync();
            ShareDirectoryClient directory = share.GetDirectoryClient(directoryName);
            await directory.CreateIfNotExistsAsync();
            if(await directory.ExistsAsync())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task UploadFile(string connectionString, string fileShareName, string directoryName, IFormFile iFormFile)
        {
            ShareClient share = new ShareClient(connectionString, fileShareName);
            await share.CreateIfNotExistsAsync();

            if(await share.ExistsAsync())
            {
                ShareDirectoryClient directory = share.GetDirectoryClient(directoryName);
                await directory.CreateIfNotExistsAsync();

                if(await directory.ExistsAsync())
                {
                    ShareFileClient file = directory.GetFileClient(iFormFile.FileName);

                    using (var stream = iFormFile.OpenReadStream())
                    {
                        file.Create(stream.Length);
                        await file.UploadRangeAsync(new HttpRange(0, iFormFile.Length), stream);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error occured in file uploading");
            }
        }

        public async Task<byte[]> DownloadFile(string connectionString, string fileShareName, string directoryName, string fileName)
        {
            ShareClient share = new ShareClient(connectionString, fileShareName);
            ShareDirectoryClient directory = share.GetDirectoryClient(directoryName);
            ShareFileClient file = directory.GetFileClient(fileName);


            var response = await file.DownloadAsync();
            using var memoryStream = new MemoryStream();
            await response.Value.Content.CopyToAsync(memoryStream);
            return memoryStream.ToArray();            
        }

        public async Task DeleteFile(string connectionString, string fileShareName, string directoryName, string fileName)
        {
            ShareClient share = new ShareClient(connectionString, fileShareName);
            ShareDirectoryClient directory = share.GetDirectoryClient(directoryName);
            ShareFileClient file = directory.GetFileClient(fileName);

            await file.DeleteIfExistsAsync(); 
        }

        public async Task<bool> DeleteDirectory(string connectionString, string fileShareName, string directoryName)
        {
            ShareClient share = new ShareClient(connectionString, fileShareName);
            ShareDirectoryClient directory = share.GetDirectoryClient(directoryName);
            if (await directory.ExistsAsync())
            {
                Console.WriteLine($"Delete directory successfully: {directoryName}");
                await directory.DeleteAsync();
                return true;
            }
            else
            {
                Console.WriteLine("Error in deleting directory");
                return false;
            }
        }
    }
}
