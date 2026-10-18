using Microsoft.EntityFrameworkCore;

namespace GymManagmentSystem_DAL.Models
{
    [Owned]
    public class Address
    {
        public int BuildingNo { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
