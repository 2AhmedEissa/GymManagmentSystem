using GymManagmentSystem_DAL.Data.Models;
using System.Linq.Expressions;

namespace GymManagmentSystem_DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity, new()
    {
        Task<IEnumerable<T>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<int> AddAsync(T entity, CancellationToken ct = default);
        Task<int> UpdateAsync(T entity, CancellationToken ct = default);
        Task<int> DeleteAsync(T entity, CancellationToken ct = default);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    }
}
