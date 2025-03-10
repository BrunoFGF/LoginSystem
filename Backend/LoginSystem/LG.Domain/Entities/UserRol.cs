namespace LG.Domain.Entities;

public partial class UserRol : BaseEntity
{
    public int UserId { get; set; }

    public virtual Rol Rol { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
