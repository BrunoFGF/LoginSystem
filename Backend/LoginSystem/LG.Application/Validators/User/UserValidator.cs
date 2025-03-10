using FluentValidation;
using LG.Application.Dtos.User.Request;

namespace LG.Application.Validators.User
{
    public class UserValidator : AbstractValidator<UserRequestDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotNull().WithMessage("El campo Nombre no puede ser nulo.")
                .NotEmpty().WithMessage("El campo Nombre no puede ser vacío.");
            RuleFor(x => x.LastName)
                .NotNull().WithMessage("El campo Apellido no puede ser nulo.")
                .NotEmpty().WithMessage("El campo Apellido no puede ser vacío.");
            RuleFor(x => x.Username)
                .NotNull().WithMessage("El campo Nombre de Usuario no puede ser nulo.")
                .NotEmpty().WithMessage("El campo Nombre de Usuario no puede ser vacío.");
        }
    }
}
