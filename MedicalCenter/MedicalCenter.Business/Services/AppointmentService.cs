using MedicalCenter.Business.Interfaces;
using MedicalCenter.Data.Entities;
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
    }
}
