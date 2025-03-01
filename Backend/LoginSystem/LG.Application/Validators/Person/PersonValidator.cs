using FluentValidation;
using LG.Application.Dtos.Person.Request;

namespace LG.Application.Validators.Person
{
    public class PersonValidator : AbstractValidator<PersonRequestDto>
    {
        public PersonValidator()
        {
            RuleFor(x => x.FirstName)
                .NotNull().WithMessage("El campo Nombre no puede ser nulo.")
                .NotEmpty().WithMessage("El campo Nombre no puede ser vacío.");
            RuleFor(x => x.LastName)
                .NotNull().WithMessage("El campo Apellido no puede ser nulo.")
                .NotEmpty().WithMessage("El campo Apellido no puede ser vacío.");
        }
    }
}
