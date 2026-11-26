using AutoMapper;
using GymManagmentSystem_BLL.Services.Interfaces;
using GymManagmentSystem_BLL.ViewModels.Plan;
using GymManagmentSystem_DAL.Data.Models;
using GymManagmentSystem_DAL.Repositories.Interfaces;

namespace GymManagmentSystem_BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);

            if (!plans.Any()) return [];

            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);
        }

        public async Task<PlanViewModel?> GetPlanDetailsAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);

            if (plan is null) return null;

            return _mapper.Map<PlanViewModel>(plan);
        }

        public async Task<EditPlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);

            if (plan is null) return null;

            var hasActiveMemberships = await _unitOfWork.GetRepository<Membership>().AnyAsync(
                m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);

            if (hasActiveMemberships) return null;

            return _mapper.Map<EditPlanViewModel>(plan);
        }

        public async Task<bool> TogglePlanAsync(int planId, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Plan>();
            var plan = await repo.GetByIdAsync(planId, ct);

            if (plan is null) return false;

            if (plan.IsActive)
            {
                var hasActiveMemberships = await _unitOfWork.GetRepository<Membership>().AnyAsync(
                    m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);

                if (hasActiveMemberships) return false;
            }

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;

            repo.UpdateAsync(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> UpdatePlanDetailsAsync(int planId, EditPlanViewModel model, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Plan>();
            var plan = await repo.GetByIdAsync(planId, ct);
            if (plan is null) return false;

            var hasActiveMemberships = await _unitOfWork.GetRepository<Membership>().AnyAsync(
                m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);

            if (hasActiveMemberships) return false;

            _mapper.Map(model, plan);
            plan.UpdatedAt = DateTime.Now;

            repo.UpdateAsync(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
    }
}
