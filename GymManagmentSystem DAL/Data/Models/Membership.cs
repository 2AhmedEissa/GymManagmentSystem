namespace GymManagmentSystem_DAL.Data.Models
{
    public class Membership : BaseEntity
    {
        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
        public bool isActive { get; set; }
        public DateTime EndDate { get; set; }

        #region Relationships

        public Member Member { get; set; } = null!;

        public int MemberId { get; set; }

        public Plan Plan { get; set; } = null!;

        public int PlanId { get; set; }

        #endregion
    }
}
