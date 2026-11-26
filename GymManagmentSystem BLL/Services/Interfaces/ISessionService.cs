using GymManagmentSystem_BLL.ViewModels.Session;

namespace GymManagmentSystem_BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct);
        Task<SessionViewModel> GetSessionDetailsAsync(int sessionId, CancellationToken ct);
        Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct);
        Task<EditSessionViewModel> GetSessionByIdForEditAsync(int sessionId, EditSessionViewModel model, CancellationToken ct);
        Task<bool> EditSessionAsync(EditSessionViewModel model, CancellationToken ct);
        Task<bool> DeleteSessionAsync(int sessionId, CancellationToken ct);
    }
}
