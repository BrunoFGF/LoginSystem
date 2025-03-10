using LG.Application.Dtos.Person.Response;

namespace LG.Application.Dtos.Account.Response
{
    public class AccountResponseDto
    {
        public string Username { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public string? Status { get; set; }
        public PersonResponseDto? Person { get; set; }
    }
}
