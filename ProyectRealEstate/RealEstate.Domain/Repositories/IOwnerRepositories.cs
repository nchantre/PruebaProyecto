using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Repositories
{
    public interface IOwnerRepositories
    {
        Task AddAsync(Ownert ownert);
        Task UpdateAsync(Ownert ownert);
        Task DeleteAsync(string idOwner);
        Task<Ownert?> GetByIdAsync(string idOwner);
        Task<List<Ownert>> GetAllAsync();
    }
}
