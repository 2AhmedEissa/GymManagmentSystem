using GymManagmentSystem_BLL.ViewModels.Plan;

namespace GymManagmentSystem_BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);
        Task<PlanViewModel?> GetPlanDetailsAsync(int planId, CancellationToken ct = default);
        Task<EditPlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default);
        Task<bool> UpdatePlanDetailsAsync(int planId, EditPlanViewModel model, CancellationToken ct = default);
        Task<bool> TogglePlanAsync(int planId, CancellationToken ct = default);

    }
}