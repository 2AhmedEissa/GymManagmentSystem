using GymManagmentSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentSystem_DAL.Configurations
{
    internal class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("CapacityCheck", "Capacity between 1 and 25");

                tb.HasCheckConstraint("DateCheck", "EndDate > StartDate");
            });
        }
    }
}
