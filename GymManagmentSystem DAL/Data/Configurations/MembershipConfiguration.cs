using Microsoft.EntityFrameworkCore;
using GymManagmentSystem_DAL.Data.Models;

namespace GymManagmentSystem_DAL.Data.Configurations
{
    internal class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Membership> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.CreatedAt)
                .HasColumnName("StartDate")
                .HasDefaultValueSql("GetDate()");
        }
    }
}
