using GymManagmentSystem.DbContexts;
using GymManagmentSystem.Models;
using GymManagmentSystem_DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagmentSystem_DAL.Repositories
{
    public class PlanRepository : IPlanRepository
    {

        private readonly GymDbContext _dbContext;

        public PlanRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            var plans = tracking ? _dbContext.Plans.ToListAsync(ct) : _dbContext.Plans.AsNoTracking().ToListAsync(ct);
            return await plans;
        }

        public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var plan = _dbContext.Plans.FindAsync(id, ct);
            return await plan;
        }
        public async Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        {
            _dbContext.Add(plan);
            return await _dbContext.SaveChangesAsync(ct);

        }

        public async Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        {
            _dbContext.Update(plan);
            return await _dbContext.SaveChangesAsync(ct);
        }


        public async Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            _dbContext.Remove(plan);
            return await _dbContext.SaveChangesAsync(ct);
        }
    }
}
