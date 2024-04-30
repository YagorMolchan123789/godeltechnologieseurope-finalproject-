using MedicalCenter.Data.Entities;

namespace MedicalCenter.Data.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment> CreateAsync(Appointment appointment);
        Task<bool> IsAnyAsync(string doctorId, DateOnly dateOnly, int timeSlotId);
    }
}
