namespace LG.Domain.Entities;

public partial class Rol : BaseEntity
{
    public string RolName { get; set; } = null!;

    public virtual ICollection<RolRolOption> RolRolOptions { get; set; } = new List<RolRolOption>();

    public virtual ICollection<UserRol> UserRols { get; set; } = new List<UserRol>();
}
