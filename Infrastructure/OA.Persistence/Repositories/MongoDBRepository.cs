using MongoDB.Driver;
using OA.Domain.Entities;

namespace OA.Persistence.Repositories
{
    public class MongoDBRepository<T> : IMongoDBRepository<T>
    {
        private readonly IMongoDatabase _db;

        public MongoDBRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017/");
            _db = client.GetDatabase("OALog");
        }

        public async Task InsertAsync(string document)
        {
            var log = new RabbitLog
            {
                Message = document
            };
            await _db.GetCollection<RabbitLog>("OALogv1").InsertOneAsync(log);
        }
    }   
}
