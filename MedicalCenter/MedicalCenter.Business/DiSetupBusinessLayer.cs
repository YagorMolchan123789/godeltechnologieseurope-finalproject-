using MedicalCenter.Business.Services.Interfaces;
using MedicalCenter.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using MedicalCenter.Business.Interfaces;

namespace MedicalCenter.Business
{
    public static class DiSetupBusinessLayer
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddTransient<ITimeSlotService, TimeSlotService>();
            services.AddTransient<IAppointmentService, AppointmentService>();

            return services;
        }
    }
}
