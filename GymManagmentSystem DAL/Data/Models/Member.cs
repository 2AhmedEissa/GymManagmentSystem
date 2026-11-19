namespace GymManagmentSystem_DAL.Data.Models
{
    public class Member : User
    {
        public string? Photo { get; set; } = null!;

        #region Relationships

        public HealthRecord HealthRecord { get; set; } = null!;

        public ICollection<Membership> Memberships { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = null!;

        #endregion
    }
}
