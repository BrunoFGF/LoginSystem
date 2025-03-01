using System.ComponentModel.DataAnnotations;

namespace LG.Application.Dtos.User.Request
{
    public class UserRequestDto
        {
            [Required(ErrorMessage = "El nombre de usuario es requerido")]
            [StringLength(20, MinimumLength = 8, ErrorMessage = "El nombre de usuario debe tener entre 8 y 20 caracteres")]
            [RegularExpression(@"^(?!.*[\W_])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,20}$",
            ErrorMessage = "El nombre de usuario debe contener al menos una letra mayúscula, un número y no debe incluir signos.")]
            public string Username { get; set; } = null!;

            [Required(ErrorMessage = "La contraseña es requerida")]
            [StringLength(16, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 50 caracteres")]
            [RegularExpression(@"^(?!.*\s)(?=.*[A-Z])(?=.*[\W_]).{8,16}$",
            ErrorMessage = "La contraseña debe tener al menos una letra mayúscula, un signo y no debe contener espacios.")]
            public string Password { get; set; } = null!;
            public string? Status { get; set; }
            public int PersonId { get; set; }
        }
    }
