using System.ComponentModel.DataAnnotations;

namespace LG.Application.Dtos.User.Request
{
    public class UpdateUserRequestDto : UserRequestDto
    {
        [StringLength(16, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 50 caracteres")]
        [RegularExpression(@"^(?!.*\s)(?=.*[A-Z])(?=.*[\W_]).{8,16}$",
        ErrorMessage = "La contraseña debe tener al menos una letra mayúscula, un signo y no debe contener espacios.")]
        public string? Password { get; set; }
    }
}
