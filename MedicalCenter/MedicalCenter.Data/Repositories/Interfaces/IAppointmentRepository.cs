using MedicalCenter.Data.Entities;
using MedicalCenter.Data.Models;

namespace MedicalCenter.Data.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetByUserIdAsync(string userId);
        Task<Appointment> CreateAsync(Appointment appointment);
        Task<IReadOnlyList<AppointmentView>> GetUserAppointmentsAsync(string userId);
        Task<bool> IsAnyAsync(string doctorId, DateOnly dateOnly, int timeSlotId);

        Task DeleteAsync(Appointment appointment);
    }
}
