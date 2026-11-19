namespace GymManagmentSystem_DAL.Data.Models
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = null!;

        public int Capacity { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        #region Relationships

        public Trainer Trainer { get; set; } = null!;

        public int TrainerId { get; set; }

        public Category Category { get; set; } = null!;

        public int CategoryId { get; set; }

        public ICollection<Booking> Bookings { get; set; } = null!;
        #endregion
    }
}
