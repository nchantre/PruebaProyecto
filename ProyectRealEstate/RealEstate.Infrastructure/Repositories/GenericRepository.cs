using MongoDB.Bson;
using MongoDB.Driver;
using RealEstate.Domain.Exceptions;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Database;
using System.Linq.Expressions;

namespace RealEstate.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;
        private readonly string _entityName;

        public GenericRepository(MongoDbContext context, string collectionName)
        {
            _collection = context.GetCollection<T>(collectionName);
            _entityName = typeof(T).Name;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _collection.Find(_ => true).ToListAsync();
            }
            catch (MongoException ex)
            {
                throw new Exception($"Error retrieving all {_entityName} from DB.", ex);
            }
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id, out objectId))
                throw new InvalidIdFormatException(id);

            try
            {
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                var entity = await _collection.Find(filter).FirstOrDefaultAsync();

                if (entity is null)
                    throw new EntityNotFoundException(_entityName, id);

                return entity;
            }
            catch (MongoException ex)
            {
                throw new Exception($"Error retrieving {_entityName} with Id {id}.", ex);
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _collection.Find(predicate).ToListAsync();
            }
            catch (MongoException ex)
            {
                throw new Exception($"Error finding {_entityName} with given predicate.", ex);
            }
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);
            }
            catch (MongoException ex)
            {
                throw new Exception($"Error inserting {_entityName}.", ex);
            }
        }

        public async Task UpdateAsync(string id, T entity)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id, out objectId))
                throw new InvalidIdFormatException(id);

            try
            {
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                var result = await _collection.ReplaceOneAsync(filter, entity);

                if (result.MatchedCount == 0)
                    throw new EntityNotFoundException(_entityName, id);
            }
            catch (MongoException ex)
            {
                throw new Exception($"Error updating {_entityName} with Id {id}.", ex);
            }
        }

        public async Task DeleteAsync(string id)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id, out objectId))
                throw new InvalidIdFormatException(id);

            try
            {
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                var result = await _collection.DeleteOneAsync(filter);

                if (result.DeletedCount == 0)
                    throw new EntityNotFoundException(_entityName, id);
            }
            catch (MongoException ex)
            {
                throw new Exception($"Error deleting {_entityName} with Id {id}.", ex);
            }
        }
    }
}
