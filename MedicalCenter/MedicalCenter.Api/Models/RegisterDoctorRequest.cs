using MedicalCenter.Api.Models.Interfaces;

namespace MedicalCenter.Api.Models
{
    public record RegisterDoctorRequest(string Email, string Password, string FirstName, 
        string LastName, int PracticeStartDate, string Specialization) : IRegisterRequest;
}
