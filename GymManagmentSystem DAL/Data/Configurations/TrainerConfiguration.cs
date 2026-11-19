using GymManagmentSystem_DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentSystem_DAL.Data.Configurations
{
    public class TrainerConfiguration : UserConfiguration<Trainer>, IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(t => t.CreatedAt)
                .HasColumnName("HireDate")
                .HasDefaultValueSql("GetDate()");

            base.Configure(builder);
        }
    }
}
