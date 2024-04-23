using MedicalCenter.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicalCenter.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
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
        }
    }
}
