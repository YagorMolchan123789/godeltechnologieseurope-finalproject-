using Microsoft.AspNetCore.Identity;

namespace MedicalCenter.Data.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DoctorInfo? DoctorInfo { get; set; }
        public List<Appointment> PatientAppointments { get; set; } = [];
        public List<Appointment> DoctorAppointments { get; set; } = [];
    }
}
