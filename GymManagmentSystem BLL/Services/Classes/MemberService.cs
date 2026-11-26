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

        public MemberService(IUnitOfWork unitOfWork,IMapper mapper)
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

            var membersViewModel = members.Select(m => new MemberViewModel()
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender,
            });

            return membersViewModel;
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

            if (member is null) return null;

            var memberViewModel = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Gender = member.Gender,
                Phone = member.Phone,
                DateOfBirth = member.DateOfBirth,
                Photo = member.Photo,
                Address = $"{member.Address.BuildingNo} - {member.Address.Street} - {member.Address.City}"
            };

            var membership = await _unitOfWork.GetRepository<Membership>().FirstOrDefaultAsync(m => m.MemberId == memberId && m.EndDate > DateTime.Now, ct: ct);

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
            var healthRecord = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(h => h.MemberId == memberId, ct);

            if (healthRecord is null) return null;

            return new HealthRecordViewModel()
            {
                Height = healthRecord.Height,
                Weight = healthRecord.Weight,
                BloodType = healthRecord.BloodType,
                Note = healthRecord.Note
            };
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

            if (member is null) return null;

            return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                BuildingNumber = member.Address.BuildingNo,
                Street = member.Address.Street,
                City = member.Address.City,
            };
        }

        public async Task<bool> UpdateMemberDetailsAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

            if (member is null) return false;

            var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != memberId, ct);
            var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != memberId, ct);

            if (emailExists || phoneExists)
            {
                return false;
            }

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.BuildingNo = model.BuildingNumber;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Member>().UpdateAsync(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

            if (member is null) return false;

            var futureBookings = await _unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == memberId && b.Session.StartDate > DateTime.Now, ct);

            if (futureBookings) return false;

            _unitOfWork.GetRepository<Member>().DeleteAsync(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
    }
}

