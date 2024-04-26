namespace MedicalCenter.Api.Models.Interfaces
{
    public interface IRegisterRequest
    {
        string Email { get; init; }
        string FirstName { get; init; }
        string LastName { get; init; }
        string Password { get; init; }
    }
}
