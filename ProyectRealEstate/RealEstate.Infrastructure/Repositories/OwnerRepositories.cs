using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Repositories;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Infrastructure.Repositories
{
    public class OwnerRepositories : IOwnerRepositories
    {
        private readonly IMongoCollection<Ownert> _collection;
        public OwnerRepositories(IMongoDatabase db) => _collection = db.GetCollection<Ownert>("Owner");

        public async Task AddAsync(Ownert ownert) => await _collection.InsertOneAsync(ownert);

        public async Task UpdateAsync(Ownert ownert) =>
            await _collection.ReplaceOneAsync(o => o.IdOwner == ownert.IdOwner, ownert);

        public async Task DeleteAsync(string idOwner) =>
            await _collection.DeleteOneAsync(o => o.IdOwner == idOwner);

        public async Task<Ownert?> GetByIdAsync(string idOwner) =>
            await _collection.Find(o => o.IdOwner == idOwner).FirstOrDefaultAsync();

        public async Task<List<Ownert>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();
    }
}
