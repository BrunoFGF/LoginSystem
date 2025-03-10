using LG.Domain.Entities;

namespace LG.Domain.Repositories
{
    public interface IUserRolRepository : IGenericRepository<UserRol>
    {
        Task<IEnumerable<Rol>> GetRolesByUserId(int userId);
        Task AssignRolesToUser(int userId, IEnumerable<int> roleIds, int auditUserId);
        Task RemoveUserRoles(int userId);
        Task<bool> UserHasRole(int userId, string roleName);
        Task<Rol> GetRolByName(string rolName);
    }
}
