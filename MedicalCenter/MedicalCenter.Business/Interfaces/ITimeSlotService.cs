using MedicalCenter.Data.Entities;

namespace MedicalCenter.Business.Interfaces
{
    public interface ITimeSlotService
    {
        public Task<IReadOnlyList<TimeSlot>> GetTimeSlotsAsync();
        public Task<IReadOnlyList<TimeSlot>> GetAvailableTimeSlotsAsync(string doctorId, DateOnly dateOnly);
    }
}
