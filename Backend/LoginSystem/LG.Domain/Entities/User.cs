namespace LG.Domain.Entities;

public partial class User : BaseEntity
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Mail { get; set; } = null!;

    public string? SessionActive { get; set; }

    public int PersonId { get; set; }

    public string? Status { get; set; }

    public int? FailedAttempts { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<UserRol> UserRols { get; set; } = new List<UserRol>();

    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
}
