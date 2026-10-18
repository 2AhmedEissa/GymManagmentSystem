namespace GymManagmentSystem_DAL.Models
{
    public class HealthRecord : BaseEntity
    {
        public int Height { get; set; }

        public int Weight { get; set; }

        public string BloodType { get; set; } = null!;

        public string Note { get; set; } = null!;
    }
}
