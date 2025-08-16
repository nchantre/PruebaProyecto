// Repositories/OwnerRepository.cs
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Database;

namespace RealEstate.Infrastructure.Repositories
{
    public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
    {
        public OwnerRepository(MongoDbContext context)
            : base(context, "Owner") 
        {
        }
    }
}
