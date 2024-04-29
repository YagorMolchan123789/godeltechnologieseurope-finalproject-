using MedicalCenter.Data.Entities;
using MedicalCenter.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalCenter.Data.Repositories
{
    public class TimeSlotRepository(ApplicationDbContext context) : ITimeSlotRepository
    {
        public async Task<IReadOnlyList<TimeSlot>> GetTimeSlotsAsync()
        {
            return await context.TimeSlots.ToListAsync();
        }

        public async Task<IReadOnlyList<TimeSlot>> GetAvailableTimeSlotsAsync(string doctorId, DateOnly dateOnly)
        {
            var reservedTimeSlots = context.Appointments
                .Where(a => a.Date == dateOnly && a.DoctorId == doctorId)
                .Select(a => a.TimeSlotId);

            var availableTimeSlots = context.TimeSlots.Where(t => !reservedTimeSlots.Contains(t.Id));

            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);

            if (dateOnly == currentDate)
            {
                availableTimeSlots = availableTimeSlots.Where(t => t.StartTime > currentTime);
            }

            return await availableTimeSlots.ToListAsync();
        }
    }
}
