using System;
using MedicalCenter.Business.Interfaces;
using MedicalCenter.Data.Entities;
using MedicalCenter.Data.Repositories.Interfaces;

namespace MedicalCenter.Business.Services
{
    public class TimeSlotService(ITimeSlotRepository timeSlotRepository) : ITimeSlotService
    {
        public async Task<IReadOnlyList<TimeSlot>> GetAvailableTimeSlotsAsync(string doctorId, DateOnly dateOnly)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            if (dateOnly < currentDate)
            {
                return [];
            }

            return await timeSlotRepository.GetAvailableTimeSlotsAsync(doctorId, dateOnly);
        }

        public async Task<IReadOnlyList<TimeSlot>> GetTimeSlotsAsync()
        {
            return await timeSlotRepository.GetTimeSlotsAsync();
        }
    }
}
