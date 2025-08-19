using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RealEstate.Domain.Exceptions;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Specifications;
using RealEstate.Infrastructure.Database;
using RealEstate.Infrastructure.Specifications;
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


        // Constructor para tests
        public GenericRepository(IMongoCollection<T> collection)
        {
            _collection = collection;
            _entityName = typeof(T).Name;
        }




        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (_collection == null)
            {
                throw new InvalidOperationException("Collection is not initialized");
            }

            var cursor = await _collection.FindAsync(_ => true);
            return await cursor.ToListAsync();
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



        public async Task<IEnumerable<T>> GetAsync(ISpecification<T> specification)
        {
            try
            {
                // USAR FindAsync EN LUGAR DE LINQ
                var filter = BuildFilterFromSpecification(specification);
                var cursor = await _collection.FindAsync(filter);
                return await cursor.ToListAsync();
            }
            catch (MongoException ex)
            {
                throw new Exception($"Error retrieving {_entityName} with specification.", ex);
            }
        }

        protected virtual IQueryable<T> GetQueryable()
        {
            return _collection.AsQueryable();
        }

        // Nuevo: lista con Specification<T, TResult> (proyección a DTO)
        public async Task<IEnumerable<TResult>> GetAsync<TResult>(ISpecification<T, TResult> specification)
        {
            try
            {
                // USAR FindAsync EN LUGAR DE LINQ PARA PROYECCIONES
                var filter = Builders<T>.Filter.Where(specification.Criteria);
                var findOptions = new FindOptions<T, TResult>
                {
                    Projection = Builders<T>.Projection.Expression(specification.Selector)
                };

                var cursor = await _collection.FindAsync(filter, findOptions);
                return await cursor.ToListAsync();
            }
            catch (MongoException ex)
            {
                throw new Exception($"Error retrieving {_entityName} with projection specification.", ex);
            }
        }

        // (Opcional) Count con spec
        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            try
            {
                // Usar CountDocumentsAsync en lugar de CountAsync de LINQ
                var filter = BuildFilterFromSpecification(specification);
                return (int)await _collection.CountDocumentsAsync(filter);
            }
            catch (MongoException ex)
            {
                throw new Exception($"Error counting {_entityName} with specification.", ex);
            }
        }

        private FilterDefinition<T> BuildFilterFromSpecification(ISpecification<T> specification)
        {
            // Implementar la conversión de tu specification a FilterDefinition
            if (specification.Criteria != null)
            {
                return new ExpressionFilterDefinition<T>(specification.Criteria);
            }
            return Builders<T>.Filter.Empty;
        }
    }
}
