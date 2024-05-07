using MedicalCenter.Data.Entities;
using MedicalCenter.Data.Mappers;
using MedicalCenter.Data.Models;
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

        public async Task DeleteAsync(Appointment appointment)
        {
            context.Appointments.Remove(appointment);
            await context.SaveChangesAsync();
        }

        public async Task DeleteByDoctorIdAsync(string doctorId)
        {
            var appointments = context.Appointments.Where(a => a.DoctorId == doctorId);
            context.Appointments.RemoveRange(appointments);
            await context.SaveChangesAsync();
        }

        public async Task<List<Appointment>> GetByUserIdAsync(string userId)
        {
            return await context.Appointments
                                .Where(x => x.PatientId == userId)
                                .ToListAsync();
        }

        public async Task<bool> IsAnyAsync(string doctorId, DateOnly dateOnly, int timeSlotId)
        {
            return await context.Appointments.AnyAsync(a => a.DoctorId == doctorId && a.Date == dateOnly && a.TimeSlotId == timeSlotId);
        }

        public async Task<IReadOnlyList<AppointmentView>> GetUserAppointmentsAsync(string userId)
        {
            var appointments = await context.Appointments
                .Include(a => a.TimeSlot)
                .Include(a => a.Doctor!)
                .ThenInclude(d => d.DoctorInfo)
                .Where(a => a.PatientId == userId)
                .OrderBy(a => a.Date)
                .ThenBy(a => a.TimeSlot.StartTime)
                .Select(a => a.ToViewModel())
                .ToListAsync();

            var doctorInfo = context.DoctorInfos.Where(d => d.AppUserId == userId);

            return appointments;
        }
    }
}
