using GymManagmentSystem_DAL.Data.DbContexts;
using GymManagmentSystem_DAL.Data.Models;
using GymManagmentSystem_DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagmentSystem_DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct = default)
        {
            var sessions = await _dbContext.Sessions
                .AsNoTracking()
                .Include(s => s.Category)
                .Include(s => s.Trainer)
                .Include(s => s.Bookings)
                .ToListAsync(ct);

            return sessions;
        }

        public async Task<Session?> GetSessionWithDetailsAsync(int id, CancellationToken ct = default)
        {
            return await _dbContext.Sessions
                .AsNoTracking()
                .Include(s => s.Category)
                .Include(s => s.Trainer)
                .Include(s => s.Bookings)
                .FirstOrDefaultAsync(s => s.Id == id, ct);
        }

        public async Task<int> GetCountOfBookeSlotsAsync(int sessionId, CancellationToken ct = default)
        {
            return await _dbContext.Bookings.CountAsync(b => b.SessionId == sessionId, ct);
        }
    }
}
