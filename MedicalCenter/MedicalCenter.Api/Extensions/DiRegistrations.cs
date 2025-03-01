﻿using FluentValidation;
using MedicalCenter.Api.Validators;
using MedicalCenter.Api.Models;
using Microsoft.AspNetCore.Identity.Data;
using MedicalCenter.Api.Helpers.Mappers;
using MedicalCenter.Api.Services;

namespace MedicalCenter.Api.Extensions
{
    public static class DiRegistrations
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterPatientRequest>, RegisterPatientRequestValidator>();
            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddScoped<IValidator<RegisterDoctorRequest>, RegisterDoctorRequestValidator>();
            services.AddScoped<DoctorInfoDtoMapper>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
