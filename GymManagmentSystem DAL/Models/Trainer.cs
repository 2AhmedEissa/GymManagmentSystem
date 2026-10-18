using GymManagmentSystem_DAL.Models.Enums;

namespace GymManagmentSystem_DAL.Models
{
    public class Trainer : User
    {
        public Specialties Specialty { get; set; }
    }
}
