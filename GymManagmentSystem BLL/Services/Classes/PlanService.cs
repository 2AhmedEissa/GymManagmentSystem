using GymManagmentSystem_BLL.Services.Interfaces;
using GymManagmentSystem_BLL.ViewModels.Plan;
using GymManagmentSystem_DAL.Data.Models;
using GymManagmentSystem_DAL.Repositories.Interfaces;

namespace GymManagmentSystem_BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);

            if (!plans.Any()) return [];

            var plansViewModel = plans.Select(p => new PlanViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                Price = p.Price,
                IsActive = p.IsActive
            });

            return plansViewModel;
        }

        public async Task<PlanViewModel?> GetPlanDetailsAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);

            if (plan is null) return null;

            return new PlanViewModel()
            {
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            };
        }
        public async Task<EditPlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);

            if (plan is null) return null;

            var hasActiveMemberships = await _unitOfWork.GetRepository<Membership>().AnyAsync(
                m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);

            if (hasActiveMemberships) return null;

            return new EditPlanViewModel()
            {
                Name = plan.Name,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                Description = plan.Description,
            };
        }

        public async Task<bool> TogglePlanAsync(int planId, CancellationToken ct)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);

            if (plan is null) return false;

            if (plan.IsActive)
            {
                var hasActiveMemberships = await _unitOfWork.GetRepository<Membership>().AnyAsync(
                    m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);

                if (hasActiveMemberships) return false;
            }

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Plan>().UpdateAsync(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> UpdatePlanDetailsAsync(int planId, EditPlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
            if (plan is null) return false;

            var hasActiveMemberships = await _unitOfWork.GetRepository<Membership>().AnyAsync(
                m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);

            if (hasActiveMemberships) return false;

            plan.DurationDays = model.DurationDays;
            plan.Price = model.Price;
            plan.Description = model.Description;
            plan.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Plan>().UpdateAsync(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
    }
}
