using GymManagmentSystem_DAL.Models.Enums;

namespace GymManagmentSystem_DAL.Models
{
    public abstract class User : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public Address Address { get; set; } = null!;

        public DateTime JoinDate { get; set; }

    }
}
