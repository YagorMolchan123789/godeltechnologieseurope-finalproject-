using MedicalCenter.Data.Entities;
using MedicalCenter.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicalCenter.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<DoctorInfo> DoctorInfos { get; set; } = null!;

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<TimeSlot> TimeSlots { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        private List<TimeSlot> GenerateTimeSlots()
        {
            const int timeSlotsCount = 8;
            const int timeMinutesInterval = 60;
            var startTime = new TimeOnly(8, 0);

            var timeSlots = new List<TimeSlot>();
            for (int i = 0; i < timeSlotsCount; i++)
            {
                var slotStartTime = startTime.AddMinutes(i * timeMinutesInterval);
                timeSlots.Add(new TimeSlot
                {
                    Id = i + 1,
                    StartTime = slotStartTime,
                    EndTime = slotStartTime.AddMinutes(timeMinutesInterval)
                });
            }
            return timeSlots;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<TimeSlot>().HasData(GenerateTimeSlots());

            builder
                .Entity<AppUser>()
                .Property(x => x.FirstName)
                .HasColumnType("nvarchar(20)");

            builder
                .Entity<AppUser>()
                .Property(x => x.LastName)
                .HasColumnType("nvarchar(20)");

            builder
                .Entity<DoctorInfo>()
                .HasOne<AppUser>()
                .WithOne()
                .HasForeignKey<DoctorInfo>(x => x.AppUserId);

            builder
                .Entity<DoctorInfo>()
                .HasKey(x => x.AppUserId);

            builder
                .Entity<DoctorInfo>()
                .HasIndex(x => x.AppUserId);

            builder
                .Entity<DoctorInfo>()
                .Property(x => x.AppUserId)
                .ValueGeneratedNever();

            builder
                .Entity<DoctorInfo>()
                .Property(x => x.AppUserId)
                .HasColumnType("nvarchar(450)");

            builder
                .Entity<DoctorInfo>()
                .Property(x => x.Specialization)
                .HasColumnType("nvarchar(50)");
        }
    }
}
