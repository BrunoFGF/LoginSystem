namespace LG.Domain.Entities;

public partial class Person : BaseEntity
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string IdentityCard { get; set; } = null!;

    public DateOnly? BirthDate { get; set; }

    public virtual User? User { get; set; }
}
