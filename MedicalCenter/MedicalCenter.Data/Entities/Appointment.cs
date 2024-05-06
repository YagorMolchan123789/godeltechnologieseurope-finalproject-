namespace MedicalCenter.Data.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public required string DoctorId { get; set; }

        public AppUser Doctor { get; set; } = null!;

        public required string PatientId { get; set; }

        public AppUser Patient { get; set; } = null!;

        public int TimeSlotId { get; set; }

        public TimeSlot TimeSlot { get; set; } = null!;

        public DateOnly Date { get; set; }

    }
}
