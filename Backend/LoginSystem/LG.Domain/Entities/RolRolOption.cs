namespace LG.Domain.Entities;

public partial class RolRolOption
{
    public int RolId { get; set; }

    public int OptionId { get; set; }

    public string? AuditCreateUser { get; set; }

    public DateTime? AuditCreateDate { get; set; }

    public string? AuditUpdateUser { get; set; }

    public DateTime? AuditUpdateDate { get; set; }

    public string? AuditDeleteUser { get; set; }

    public DateTime? AuditDeleteDate { get; set; }

    public virtual RolOption Option { get; set; } = null!;

    public virtual Rol Rol { get; set; } = null!;
}
