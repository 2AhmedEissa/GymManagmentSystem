using GymManagmentSystem_DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagmentSystem_DAL.Data.Configurations
{
    public class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.Property(h => h.BloodType)
                .HasMaxLength(3);

            builder.Property(h => h.Note)
                .HasMaxLength(500);
        }
    }
}
