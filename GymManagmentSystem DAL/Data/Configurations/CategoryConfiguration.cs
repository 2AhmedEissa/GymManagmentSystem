using GymManagmentSystem_DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagmentSystem_DAL.Data.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(b => b.Name)
                .HasColumnType("varchar")
                .HasMaxLength(20);

     builder.HasData(
        new Category { Id = 1, Name = "Cardio" },
        new Category { Id = 2, Name = "Strength" },
        new Category { Id = 3, Name = "Yoga" },
        new Category { Id = 4, Name = "Boxing" },
        new Category { Id = 5, Name = "CrossFit" }
    );
        }
    }
}
