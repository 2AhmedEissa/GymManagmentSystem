namespace GymManagmentSystem_DAL.Data.Models
{
    public class HealthRecord : BaseEntity
    {
        public decimal Height { get; set; }

        public decimal Weight { get; set; }

        public string BloodType { get; set; } = null!;

        public string? Note { get; set; } = null!;

        #region Relationships

        public Member Member { get; set; } = null!;
        public int MemberId { get; set; }

        #endregion
    }
}
