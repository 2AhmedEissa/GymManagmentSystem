using GymManagmentSystem_BLL.Common.Results;
using GymManagmentSystem_BLL.ViewModels.Session;

namespace GymManagmentSystem_BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<SessionViewModel?> GetSessionDetailsAsync(int sessionId, CancellationToken ct = default);
        Task<ServiceResult> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);
        Task<ServiceResult<UpdateSessionViewModel>> GetSessionForUpdateAsync(int id, CancellationToken ct = default);
        Task<ServiceResult> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default);
        Task<ServiceResult> DeleteSessionAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default);
    }
}
