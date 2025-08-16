// Services/OwnerService.cs
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Services
{
    public class OwnerService
    {
        private readonly IOwnerRepository _ownerRepository;

        public OwnerService(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public Task<IEnumerable<Owner>> GetAllAsync() => _ownerRepository.GetAllAsync();
        public Task<Owner> GetByIdAsync(string id) => _ownerRepository.GetByIdAsync(id);
        public Task AddAsync(Owner owner) => _ownerRepository.AddAsync(owner);
        public Task UpdateAsync(string id, Owner owner) => _ownerRepository.UpdateAsync(id, owner);
        public Task DeleteAsync(string id) => _ownerRepository.DeleteAsync(id);
    }
}
