using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Mongo;

namespace RealEstate.Infrastructure.UnitOfWork
{

    public class MongoUnitWork : IMongoUnitWork
    {
        private readonly IMongoDatabase _database;

        public MongoUnitWork(IMongoClient client, IOptions<MongoDbSettings> options)
        {
            _database = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoDatabase Database => _database;

        public IMongoCollection<T> GetCollection<T>(string name) =>
            _database.GetCollection<T>(name);
    }
}
