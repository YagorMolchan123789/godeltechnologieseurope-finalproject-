using FluentValidation;
using MedicalCenter.Api.Validators;
using MedicalCenter.Api.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace MedicalCenter.Api.Extensions
{
    public static class DiRegistrations
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterPatientRequest>, RegisterPatientRequestValidator>();
            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddScoped<IValidator<RegisterDoctorRequest>, RegisterDoctorRequestValidator>();

            return services;
        }
    }
}
