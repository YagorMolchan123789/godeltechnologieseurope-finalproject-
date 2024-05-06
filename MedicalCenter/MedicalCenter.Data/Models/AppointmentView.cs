namespace MedicalCenter.Data.Models
{
    public class AppointmentView
    {
        public int Id { get; set; }
        public AppointmentDoctorInfo? DoctorInfo { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly? Time { get; set; }
        public bool? IsPast { get; set; }
    }
}
