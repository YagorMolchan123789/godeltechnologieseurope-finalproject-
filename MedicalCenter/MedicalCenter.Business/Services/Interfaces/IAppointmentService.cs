using MedicalCenter.Data.Entities;

namespace MedicalCenter.Business.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task CreateAppointmentAsync(string patientId, CreateAppointmentModel model);
        Task<List<Appointment>> GetByUserIdAsync(string userId);
        Task DeleteAsync (string userId, int appointmentId);
    }
}
