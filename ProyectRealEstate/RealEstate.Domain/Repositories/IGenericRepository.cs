using RealEstate.Domain.Specifications;
using System.Linq.Expressions;

namespace RealEstate.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task AddAsync(T entity);
        Task UpdateAsync(string id, T entity);
        Task DeleteAsync(string id);

        Task<IEnumerable<T>> GetAsync(ISpecification<T> specification);
        Task<IEnumerable<TResult>> GetAsync<TResult>(ISpecification<T, TResult> specification); // <- nueva
        Task<int> CountAsync(ISpecification<T> specification); // opcional
    }
    
}