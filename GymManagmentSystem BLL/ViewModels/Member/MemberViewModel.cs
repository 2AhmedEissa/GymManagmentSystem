using GymManagmentSystem_DAL.Data.Models.Enums;

namespace GymManagmentSystem_BLL.ViewModels.Member
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public string? Photo { get; set; } = default!;
        public Gender Gender { get; set; }
        public string Address { get; set; } = default!;
        public string PlanName { get; set; } = default!;
        public string MembershipStartDate { get; set; } = default!;
        public string MembershipEndDate { get; set; } = default!;

    }
}
