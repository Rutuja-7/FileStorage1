namespace FileStorage.Services
{
    public interface IBlobStorage
    {
        public Task<bool> CreateContainer(string connectionSTring, string containerName);   
        public Task<List<string>> GetAllDocuments(string connectionString, string containerName);
        public Task UploadDocument(string connectionString, string containerName, string fileName, Stream containerContent);
        public Task<Stream> GetDocument(string connectionString, string containerName, string fileName);
        public Task<bool> DeleteDocument(string connectionString, string containerName, string fileName);
        public Task<bool> DeleteContainer(string connectionString, string containerName);
    }
}
