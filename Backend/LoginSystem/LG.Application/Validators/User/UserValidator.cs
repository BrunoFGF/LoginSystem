using FluentValidation;
using LG.Application.Dtos.User.Request;

namespace LG.Application.Validators.User
{
    public class UserValidator : AbstractValidator<UserRequestDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Username)
                .NotNull().WithMessage("El campo Nombre de Usuario no puede ser nulo.")
                .NotEmpty().WithMessage("El campo Nombre de Usuario no puede ser vacío.");
            RuleFor(x => x.Password)
                .NotNull().WithMessage("El campo Contraseña no puede ser nulo.")
                .NotEmpty().WithMessage("El campo Contraseña no puede ser vacío.");
        }
    }
}
