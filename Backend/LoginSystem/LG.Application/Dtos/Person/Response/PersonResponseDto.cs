namespace LG.Application.Dtos.Person.Response
{
    public class PersonResponseDto
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string IdentityCard { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        public DateTime AuditCreateDate { get; set; }
    }
}
