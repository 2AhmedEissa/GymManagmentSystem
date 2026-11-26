using GymManagmentSystem_DAL.Data.Models;
using GymManagmentSystem_DAL.Data.Models.Enums;

namespace GymManagmentSystem_BLL.ViewModels.Trainer
{
    public class CreateTrainerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Address Address { get; set; } = default!;
        public Specialties Specialization { get; set; }
    }
}
