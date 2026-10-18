using GymManagmentSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentSystem_DAL.Configurations
{
    public class UserConfiguration<T> : IEntityTypeConfiguration<T> where T : User
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(u => u.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            builder.Property(u => u.Phone)
                .HasColumnType("varchar")
                .HasMaxLength(11);

            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasIndex(u => u.Phone).IsUnique();

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("EmailCheck", "Email like '_%@_%._%'");
                tb.HasCheckConstraint("PhoneCheck", "Phone like '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");
            });

            builder.OwnsOne(u => u.Address, address =>
            {
                address.Property(a => a.Street)
                .HasColumnType("varchar")
                .HasMaxLength(30);

                address.Property(a => a.City)
                .HasColumnType("varchar")
                .HasMaxLength(30);
            });
        }
    }
}
