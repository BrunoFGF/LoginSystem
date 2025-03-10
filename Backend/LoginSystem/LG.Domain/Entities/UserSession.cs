namespace LG.Domain.Entities;

public partial class UserSession
{
    public int SessionId { get; set; }

    public DateTime EntryDate { get; set; }

    public DateTime? CloseDate { get; set; }

    public int UserId { get; set; }

    public string? AuditCreateUser { get; set; }

    public DateTime? AuditCreateDate { get; set; }

    public string? AuditUpdateUser { get; set; }

    public DateTime? AuditUpdateDate { get; set; }

    public string? AuditDeleteUser { get; set; }

    public DateTime? AuditDeleteDate { get; set; }

    public virtual User User { get; set; } = null!;
}
