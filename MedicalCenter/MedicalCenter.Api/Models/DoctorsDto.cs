namespace MedicalCenter.Api.Models
{
    public class DoctorsDto
    {
        public List<DoctorInfoDto> DoctorInfos { get; set; } = [];
        public bool IsShowButton { get; set; }
    }
}
