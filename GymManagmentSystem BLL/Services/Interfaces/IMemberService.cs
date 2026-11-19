using GymManagmentSystem_BLL.ViewModels;

namespace GymManagmentSystem_BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAlMembersAsync(CancellationToken ct = default);
        Task<bool> CreateMemberAsync(CreateMemberViewModel model,CancellationToken ct = default);

    }
}
