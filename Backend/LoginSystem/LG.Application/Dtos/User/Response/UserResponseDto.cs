using LG.Application.Dtos.Person.Response;

namespace LG.Application.Dtos.User.Response
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public string? SessionActive { get; set; }
        public string? Status { get; set; }
        public int? FailedAttempts { get; set; }
        public PersonResponseDto? Person { get; set; }
        public DateTime AuditCreateDate { get; set; }
        public string RolName { get; set; } = null!;
    }
}
