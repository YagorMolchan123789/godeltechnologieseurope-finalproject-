using FluentValidation;
using MedicalCenter.Api.Models;

namespace MedicalCenter.Api.Validators
{
    public class RegisterDoctorRequestValidator : AbstractValidator<RegisterDoctorRequest>
    {
        public RegisterDoctorRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().Length(5, 256);
            RuleFor(x => x.Password).NotEmpty().Length(8, 20);
            RuleFor(x => x.FirstName).NotEmpty().Length(1, 20);
            RuleFor(x => x.LastName).NotEmpty().Length(1, 20);
            RuleFor(x => x.PracticeStartDate).NotEmpty();
            RuleFor(x => x.Specialization).NotEmpty().Length(1, 50);
        }
    }
}
