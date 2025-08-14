using MongoDB.Driver;

namespace RealEstate.Domain.Interfaces
{
    public interface IMongoUnitWork
    {
        IMongoDatabase Database { get; }
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
