namespace GymManagmentSystem_DAL.Data.Models
{
    public class Booking : BaseEntity
    {
        public bool IsAttended { get; set; }

        #region Relationship

        public Member Member { get; set; } = null!;

        public int MemberId { get; set; }

        public Session Session { get; set; } = null!;

        public int SessionId { get; set; }


        #endregion
    }
}
