using GymManagmentSystem_DAL.Data.Models;
namespace GymManagmentSystem_DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct = default);
        Task<int> GetCountOfBookeSlotsAsync(int sessionId, CancellationToken ct = default);
    }
}
