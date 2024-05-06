using System.Runtime.CompilerServices;
using MedicalCenter.Data.Entities;
using MedicalCenter.Data.Models;

namespace MedicalCenter.Data.Mappers
{
    public static class AppointmentMapper
    {
        public static AppointmentView ToViewModel(this Appointment appointment)
        {
            return new AppointmentView
            {
                Id = appointment.Id,
                Time = appointment.TimeSlot?.StartTime,
                Date = appointment.Date,
                DoctorInfo = new AppointmentDoctorInfo
                {
                    FirstName = appointment.Doctor?.FirstName,
                    LastName = appointment.Doctor?.LastName,
                    Specialization = appointment.Doctor?.DoctorInfo?.Specialization,
                    PracticeStartDate = appointment.Doctor?.DoctorInfo?.PracticeStartDate,
                },
                IsPast = new DateTime(appointment.Date, appointment.TimeSlot!.StartTime) < DateTime.Now
            };
        }
    }
}
