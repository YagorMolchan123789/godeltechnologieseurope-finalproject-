namespace MedicalCenter.Data.Entities
{
    public class DoctorInfo
    {
        public int Id { get; set; }
        public required string AppUserId { get; set; }
        public int PracticeStartDate { get; set; }
        public string Specialization { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
    }
}
