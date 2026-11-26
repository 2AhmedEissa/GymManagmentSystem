using GymManagmentSystem_DAL.Data.Models.Enums;

namespace GymManagmentSystem_DAL.Data.Models
{
    public class Trainer : User
    {
        public Specialties Specialization { get; set; }

        #region Relationships

        public ICollection<Session> Sessions { get; set; } = null!;

        #endregion
    }
}
