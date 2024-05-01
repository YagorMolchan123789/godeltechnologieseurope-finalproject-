using MedicalCenter.Data.Entities;

namespace MedicalCenter.Api.Models
{
    public class GetAvailableTimeSlotsResponse
    {
        public IReadOnlyList<TimeSlot> timeSlots { get; set; } = Array.Empty<TimeSlot>();

        public TimeOnly CurrentTime { get; set; }
    }
}
