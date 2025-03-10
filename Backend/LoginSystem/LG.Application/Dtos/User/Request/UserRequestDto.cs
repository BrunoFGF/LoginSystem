using System.ComponentModel.DataAnnotations;

namespace LG.Application.Dtos.User.Request
{
    public class UserRequestDto
        {
            [Required(ErrorMessage = "El nombre es requerido")]
            [StringLength(80, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 80 caracteres")]
            public string FirstName { get; set; } = null!;

            [Required(ErrorMessage = "El apellido es requerido")]
            [StringLength(80, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 80 caracteres")]
            public string LastName { get; set; } = null!;

            [Required(ErrorMessage = "El documento de identidad es requerido")]
            [StringLength(10, ErrorMessage = "El documento debe tener 10 caracteres")]
            [RegularExpression("^(?!.*(\\d)\\1{3})[0-9]{10}$", ErrorMessage = "El documento debe contener solo números y no puede tener un mismo dígito repetido más de 3 veces seguidas")]
            public string IdentityCard { get; set; } = null!;

            public DateOnly? BirthDate { get; set; }


            [Required(ErrorMessage = "El nombre de usuario es requerido")]
            [StringLength(20, MinimumLength = 8, ErrorMessage = "El nombre de usuario debe tener entre 8 y 20 caracteres")]
            [RegularExpression(@"^(?!.*[\W_])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,20}$",
            ErrorMessage = "El nombre de usuario debe contener al menos una letra mayúscula, un número y no debe incluir signos.")]
            public string Username { get; set; } = null!;

            public string? Status { get; set; }

            [Required(ErrorMessage = "El rol es requerido")]
            public string RolName { get; set; } = null!;
    }
    }