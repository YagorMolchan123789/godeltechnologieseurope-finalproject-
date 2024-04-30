namespace MedicalCenter.Api.Models
{
    public class DoctorInfoDto
    {
        public string AppUserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int PracticeStartDate { get; set; }
        public string Specialization { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
    }
}
