﻿using MedicalCenter.Data.Entities;
using MedicalCenter.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalCenter.Data.Repositories
{
    public class AppointmentRepository(ApplicationDbContext context) : IAppointmentRepository
    {
        public async Task<Appointment> CreateAsync(Appointment appointment)
        {
            await context.Appointments.AddAsync(appointment);
            await context.SaveChangesAsync();
            
            return appointment;
        }

        public async Task<bool> IsAnyAsync(DateOnly dateOnly, int timeSlotId)
        {
            return await context.Appointments.AnyAsync(a => a.Date == dateOnly && a.TimeSlotId == timeSlotId);
        }
    }
}
