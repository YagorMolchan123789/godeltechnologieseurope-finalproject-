using FluentValidation;
using MedicalCenter.Api.Models;

namespace MedicalCenter.Api.Validators
{
    public class RegisterPatientRequestValidator : AbstractValidator<RegisterPatientRequest>
    {
        public RegisterPatientRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().Length(5, 256);
            RuleFor(x => x.Password).NotEmpty().Length(8, 20);
            RuleFor(x => x.FirstName).NotEmpty().Length(1, 20);
            RuleFor(x => x.LastName).NotEmpty().Length(1, 20);
        }
    }
}
