using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace MedicalCenter.Api.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().Length(8, 20);
        }
    }
}
