using MedicalCenter.Business.Services.Interfaces;
using MedicalCenter.Data.Entities;
using MedicalCenter.Data.Models;
using MedicalCenter.Data.Repositories.Interfaces;

namespace MedicalCenter.Business.Services
{
    public class AppointmentService(
        IAppointmentRepository appointmentRepository
        ) : IAppointmentService
    {
        public async Task CreateAppointmentAsync(string patientId, CreateAppointmentModel model)
        {
            if (await appointmentRepository.IsAnyAsync(model.DoctorId, model.Date, model.TimeSlotId))
            {
                throw new ArgumentException("This time already reserved");
            }

            var appointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = model.DoctorId,
                TimeSlotId = model.TimeSlotId,
                Date = model.Date,
            };

            await appointmentRepository.CreateAsync(appointment);
        }

        public Task<List<Appointment>> GetByUserIdAsync(string userId)
        {
            return appointmentRepository.GetByUserIdAsync(userId);
        }

        public async Task DeleteAsync(string userId, int appointmentId)
        {
            var appointment = (await appointmentRepository.GetByUserIdAsync(userId)).FirstOrDefault(x => x.Id == appointmentId);

            if (appointment != null)
            {
                await appointmentRepository.DeleteAsync(appointment);
            }
        }
        
        public async Task<IReadOnlyList<AppointmentView>> GetUserAppointmentsAsync(string userId)
        {
            return await appointmentRepository.GetUserAppointmentsAsync(userId);
        }
    }
}
