using MedicalCenter.Data.Entities;

namespace MedicalCenter.Business.Interfaces
{
    public interface IAppointmentService
    {
        Task CreateAppointmentAsync(string patientId, CreateAppointmentModel model);
    }
}
