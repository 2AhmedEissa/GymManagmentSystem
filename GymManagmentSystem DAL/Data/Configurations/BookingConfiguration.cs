using GymManagmentSystem_DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagmentSystem_DAL.Data.Configurations
{
    internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(b => b.Id);
            builder.HasKey(b => new { b.MemberId, b.SessionId });
            builder.Property(b => b.CreatedAt)
                .HasColumnName("BookingDate")
                .HasDefaultValueSql("GetDate()");
        }
    }
}
