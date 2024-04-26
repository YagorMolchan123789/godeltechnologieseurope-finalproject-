using MedicalCenter.Api.Models.Interfaces;

namespace MedicalCenter.Api.Models
{
    public record RegisterPatientRequest(string Email, string Password, string FirstName,
        string LastName) : IRegisterRequest;
}
