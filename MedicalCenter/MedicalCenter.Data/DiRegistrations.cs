using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalCenter.Data
{
    public static class DiRegistrations
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, string? connectionString, bool isDevelopment)
        {
            if (isDevelopment)
            {
                services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connectionString)
                    .EnableSensitiveDataLogging());
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connectionString));
            }

            return services;
        }
    }
}
