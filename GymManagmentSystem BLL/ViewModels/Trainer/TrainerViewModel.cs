using GymManagmentSystem_DAL.Data.Models;
using GymManagmentSystem_DAL.Data.Models.Enums;

namespace GymManagmentSystem_BLL.ViewModels.Trainer
{
    public class TrainerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; } = default!;
        public Specialties Specialization { get; set; }
    }
}
