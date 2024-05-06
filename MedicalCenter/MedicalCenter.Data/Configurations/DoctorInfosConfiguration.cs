using MedicalCenter.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalCenter.Data.Configurations
{
    public class DoctorInfosConfiguration : IEntityTypeConfiguration<DoctorInfo>
    {
        public void Configure(EntityTypeBuilder<DoctorInfo> builder)
        {
            builder
                .HasOne<AppUser>()
                .WithOne(u => u.DoctorInfo)
                .HasForeignKey<DoctorInfo>(x => x.AppUserId)
                .IsRequired();

            builder
                .Property(x => x.AppUserId)
                .HasColumnType("nvarchar(36)");

            builder
                .Property(x => x.Specialization)
                .HasColumnType("nvarchar(50)")
                .IsRequired();

            builder
                .Property(x => x.PhotoUrl)
                .HasColumnType("nvarchar(50)");
        }
    }
}
