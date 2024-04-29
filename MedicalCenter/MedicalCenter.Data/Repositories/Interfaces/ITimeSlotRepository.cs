using MedicalCenter.Data.Entities;

namespace MedicalCenter.Data.Repositories.Interfaces
{
    public interface ITimeSlotRepository
    {
        public Task<IReadOnlyList<TimeSlot>> GetAvailableTimeSlotsAsync(string doctorId, DateOnly dateOnly);
        public Task<IReadOnlyList<TimeSlot>> GetTimeSlotsAsync();
    }
}
