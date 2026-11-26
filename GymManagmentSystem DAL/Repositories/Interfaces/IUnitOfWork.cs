using GymManagmentSystem_DAL.Data.Models;

namespace GymManagmentSystem_DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        public ISessionRepository SessionRepository { get; }
    }
}
