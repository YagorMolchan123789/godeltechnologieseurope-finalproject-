using MedicalCenter.Data.Repositories;
using MedicalCenter.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalCenter.Data
{
    public static class DiSetupDataLayer
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

            services.AddTransient<ITimeSlotRepository, TimeSlotRepository>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();

            return services;
        }
    }
}
