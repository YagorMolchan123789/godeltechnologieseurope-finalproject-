namespace MedicalCenter.Data.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public required string DoctorId { get; set; }

        public required string PatientId { get; set; }

        public int TimeSlotId { get; set; }

        public DateOnly Date { get; set; }

    }
}
