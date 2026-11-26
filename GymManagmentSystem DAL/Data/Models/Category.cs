namespace GymManagmentSystem_DAL.Data.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;

        #region Relationships

        public ICollection<Session> Sessions { get; set; } = null!;

        


        #endregion
    }
}
