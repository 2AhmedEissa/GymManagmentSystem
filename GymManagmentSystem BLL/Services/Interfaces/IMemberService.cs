using GymManagmentSystem_BLL.ViewModels;
using GymManagmentSystem_BLL.ViewModels.Member;

namespace GymManagmentSystem_BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default);
        Task<HealthRecordViewModel?> GetHealthRecordDetailsAsync(int memberId, CancellationToken ct = default);
        Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default);
        Task<bool> UpdateMemberDetailsAsync(int memberId, MemberToUpdateViewModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default);
    }
}
