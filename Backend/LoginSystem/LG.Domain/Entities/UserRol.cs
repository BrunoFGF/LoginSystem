using System;
using System.Collections.Generic;

namespace LG.Domain.Entities;

public partial class UserRol
{
    public int RolId { get; set; }

    public int UserId { get; set; }

    public string? AuditCreateUser { get; set; }

    public DateTime? AuditCreateDate { get; set; }

    public string? AuditUpdateUser { get; set; }

    public DateTime? AuditUpdateDate { get; set; }

    public string? AuditDeleteUser { get; set; }

    public DateTime? AuditDeleteDate { get; set; }

    public virtual Rol Rol { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
