using MedicalCenter.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicalCenter.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<DoctorInfo> DoctorInfos { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
