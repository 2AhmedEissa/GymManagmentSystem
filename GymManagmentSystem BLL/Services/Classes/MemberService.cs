using AutoMapper;
using GymManagmentSystem_BLL.Services.Interfaces;
using GymManagmentSystem_BLL.ViewModels;
using GymManagmentSystem_BLL.ViewModels.Member;
using GymManagmentSystem_DAL.Data.Models;
using GymManagmentSystem_DAL.Repositories.Interfaces;

namespace GymManagmentSystem_BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Member>();
            var emailExists = await repo.AnyAsync(m => m.Email == model.Email, ct);
            var phoneExists = await repo.AnyAsync(m => m.Phone == model.Phone, ct);

            if (emailExists || phoneExists)
            {
                return false;
            }

            var member = _mapper.Map<Member>(model);

            repo.AddAsync(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);

            if (!members.Any()) return [];

            return _mapper.Map<IEnumerable<MemberViewModel>>(members);
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

            if (member is null) return null;

            var memberViewModel = _mapper.Map<MemberViewModel>(member);

            var membership = await _unitOfWork.GetRepository<Membership>()
                .FirstOrDefaultAsync(m => m.MemberId == memberId && m.EndDate > DateTime.Now, ct: ct);

            if (membership is not null)
            {
                var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(membership.PlanId, ct);
                memberViewModel.PlanName = plan?.Name!;
                memberViewModel.MembershipStartDate = membership.CreatedAt.ToShortDateString();
                memberViewModel.MembershipEndDate = membership.EndDate.ToShortDateString();
            }

            return memberViewModel;
        }

        public async Task<HealthRecordViewModel?> GetHealthRecordDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var healthRecord = await _unitOfWork.GetRepository<HealthRecord>()
                .FirstOrDefaultAsync(h => h.MemberId == memberId, ct);

            if (healthRecord is null) return null;

            return _mapper.Map<HealthRecordViewModel>(healthRecord);
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

            if (member is null) return null;

            return _mapper.Map<MemberToUpdateViewModel>(member);
        }

        public async Task<bool> UpdateMemberDetailsAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Member>();
            var member = await repo.GetByIdAsync(memberId, ct);

            if (member is null) return false;

            var emailExists = await repo.AnyAsync(m => m.Email == model.Email && m.Id != memberId, ct);
            var phoneExists = await repo.AnyAsync(m => m.Phone == model.Phone && m.Id != memberId, ct);

            if (emailExists || phoneExists)
            {
                return false;
            }

            _mapper.Map(model, member);
            member.UpdatedAt = DateTime.Now;

            repo.UpdateAsync(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Member>();
            var member = await repo.GetByIdAsync(memberId, ct);

            if (member is null) return false;

            var futureBookings = await _unitOfWork.GetRepository<Booking>()
                .AnyAsync(b => b.MemberId == memberId && b.Session.StartDate > DateTime.Now, ct);

            if (futureBookings) return false;

            repo.DeleteAsync(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
    }
}
