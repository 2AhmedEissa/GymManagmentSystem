using GymManagmentSystem_DAL.Data.DbContexts;
using GymManagmentSystem_DAL.Data.Models;
using GymManagmentSystem_DAL.Repositories.Interfaces;

namespace GymManagmentSystem_DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<string, object> repositories = [];
        public ISessionRepository SessionRepository { get; }

        public UnitOfWork(GymDbContext dbContext, ISessionRepository sessionRepository)
        {
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var typeName = typeof(TEntity).Name;

            if (repositories.TryGetValue(typeName, out var repository))
            {
                return (IGenericRepository<TEntity>)repository;
            }

            var repo = new GenericRepository<TEntity>(_dbContext);

            repositories[typeName] = repo;

            return repo;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _dbContext.SaveChangesAsync(ct);
        }
    }
}
