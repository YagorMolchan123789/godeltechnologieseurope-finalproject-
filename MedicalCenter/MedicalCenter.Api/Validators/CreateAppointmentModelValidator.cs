using FluentValidation;
using MedicalCenter.Business;

namespace MedicalCenter.Api.Validators
{
    public class CreateAppointmentModelValidator : AbstractValidator<CreateAppointmentModel>
    {
        public CreateAppointmentModelValidator()
        {
            RuleFor(model => model.Date.ToDateTime(new TimeOnly())).GreaterThanOrEqualTo(DateTime.UtcNow);
        }

    }
}
