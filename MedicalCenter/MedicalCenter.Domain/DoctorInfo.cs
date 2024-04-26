namespace MedicalCenter.Domain
{
    public class DoctorInfo
    {
        public string AppUserId { get; set; } = string.Empty;
        public int PracticeStartDate { get; set; }
        public string Specialization { get; set; } = string.Empty;
    }
}
