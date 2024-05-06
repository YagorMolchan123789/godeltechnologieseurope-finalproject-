using MedicalCenter.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalCenter.Data.Configurations
{
    public class AppUsersConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder
                .Property(x => x.FirstName)
                .HasColumnType("nvarchar(20)");

            builder
                .Property(x => x.LastName)
                .HasColumnType("nvarchar(20)");

            builder
                .HasMany(x => x.PatientAppointments)
                .WithOne(x => x.Patient)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder
                .HasMany(x => x.DoctorAppointments)
                .WithOne(x => x.Doctor)
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
