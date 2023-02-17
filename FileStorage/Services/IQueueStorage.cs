namespace FileStorage.Services
{
    public interface IQueueStorage
    {
        public Task<bool> CreateQueue(string connectionString, string queueName);
        public Task InsertMessage(string connectionString, string queueName, string message);
        public Task UpdateMessage(string connectionString, string queueName, string message);
        public Task ReadMessage(string connectionString, string queueName);
        public Task<bool> DeleteMessage(string connectionString, string queueName);
        public Task<bool> DeleteQueue(string connectionString, string queueName);
    }
}
