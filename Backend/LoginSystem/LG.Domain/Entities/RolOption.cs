namespace LG.Domain.Entities;

public partial class RolOption
{
    public int OptionId { get; set; }

    public string? AuditCreateUser { get; set; }

    public DateTime? AuditCreateDate { get; set; }

    public string? AuditUpdateUser { get; set; }

    public DateTime? AuditUpdateDate { get; set; }

    public string? AuditDeleteUser { get; set; }

    public DateTime? AuditDeleteDate { get; set; }

    public string OptionName { get; set; } = null!;

    public virtual ICollection<RolRolOption> RolRolOptions { get; set; } = new List<RolRolOption>();
}
