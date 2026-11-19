using GymManagmentSystem_DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentSystem_DAL.Data.Configurations
{
    public class MemberConfiguration : UserConfiguration<Member>, IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(m => m.CreatedAt)
                .HasColumnName("JoinDate")
                .HasDefaultValueSql("GetDate()");

            base.Configure(builder);

        }
    }
}
