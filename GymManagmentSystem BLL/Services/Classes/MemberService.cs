
using GymManagmentSystem_BLL.Services.Interfaces;
using GymManagmentSystem_BLL.ViewModels;
using GymManagmentSystem_DAL.Data.Models;
using GymManagmentSystem_DAL.Repositories.Interfaces;

namespace GymManagmentSystem_BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepo;

        public MemberService(IGenericRepository<Member> memberRepo)
        {
            _memberRepo = memberRepo;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailExists = await _memberRepo.AnyAsync(m => m.Email == model.Email, ct);
            var phoneExists = await _memberRepo.AnyAsync(m => m.Email == model.Phone, ct);

            if (emailExists || phoneExists)
            {
                return false;
            }

            var member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Address = new Address()
                {
                    BuildingNo = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                HealthRecord = new HealthRecord()
                {
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note
                }
            };
            var result = await _memberRepo.AddAsync(member, ct);
            return result > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAlMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberRepo.GetAllAsync(ct: ct);

            if (!members.Any())
            {
                return [];
            }

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
    }
}
