namespace MedicalCenter.Business
{
    public class CreateAppointmentModel
    {
        public required string DoctorId { get; set; }
        public int TimeSlotId { get; set; }
        public DateOnly Date { get; set; }
    }
}
