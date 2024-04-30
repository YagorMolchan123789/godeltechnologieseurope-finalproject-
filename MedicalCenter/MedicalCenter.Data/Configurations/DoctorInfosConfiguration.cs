using MedicalCenter.Domain;
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
                .WithOne()
                .HasForeignKey<DoctorInfo>(x => x.AppUserId);

            builder
                .HasKey(x => x.AppUserId);

            builder
                .HasIndex(x => x.AppUserId);

            builder
                .Property(x => x.AppUserId)
                .ValueGeneratedNever();

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
