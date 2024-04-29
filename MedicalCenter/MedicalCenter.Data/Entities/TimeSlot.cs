namespace MedicalCenter.Data.Entities
{
    public class TimeSlot
    {
        public int Id { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
    }
}
