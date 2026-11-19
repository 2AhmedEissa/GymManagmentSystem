using GymManagmentSystem_DAL.Data.DbContexts;
using GymManagmentSystem_DAL.Data.Models;
using GymManagmentSystem_DAL.Repositories.Classes;
using GymManagmentSystem_DAL.Repositories.Interfaces;

namespace GymManagmentSystem_DAL.Repositories
{
    public class PlanRepository : GenericRepository<Plan>
    {
        public PlanRepository(GymDbContext dbContext) : base(dbContext)
        {
        }

        public Task<IEnumerable<Plan>> GetAllWithMembershipsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
