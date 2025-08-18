using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Interfaces
{
    public interface IOwnerService
    {
        Task<IEnumerable<Owner>> GetAllAsync();
        Task<Owner> GetByIdAsync(string id);
        Task AddAsync(Owner owner);
        Task UpdateAsync(string id, Owner owner);
        Task DeleteAsync(string id);
    }
}
