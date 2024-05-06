using MedicalCenter.Data.Entities;
using MedicalCenter.Data.Models;

namespace MedicalCenter.Business.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task CreateAppointmentAsync(string patientId, CreateAppointmentModel model);
        Task<List<Appointment>> GetByUserIdAsync(string userId);
        Task DeleteAsync (string userId, int appointmentId);
        Task<IReadOnlyList<AppointmentView>> GetUserAppointmentsAsync(string userId);
    }
}
