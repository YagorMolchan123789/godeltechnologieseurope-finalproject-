using MedicalCenter.Business.Interfaces;
using MedicalCenter.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalCenter.Data
{
    public static class DiRegistrations
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddTransient<ITimeSlotService, TimeSlotService>();
            services.AddTransient<IAppointmentService, AppointmentService>();

            return services;
        }
    }
}
