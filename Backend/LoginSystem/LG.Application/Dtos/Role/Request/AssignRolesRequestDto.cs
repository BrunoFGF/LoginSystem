namespace LG.Application.Dtos.Role.Request
{
    public class AssignRolesRequestDto
    {
        public int UserId { get; set; }
        public IEnumerable<int> RoleIds { get; set; } = null!;
        public int AuditUserId { get; set; }
    }
}
