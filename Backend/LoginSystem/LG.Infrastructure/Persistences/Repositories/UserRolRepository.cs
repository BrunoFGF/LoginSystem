using LG.Domain.Entities;
using LG.Domain.Repositories;
using LG.Domain.Services;
using LG.Infrastructure.Persistences.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LG.Infrastructure.Persistences.Repositories
{
    public class UserRolRepository : GenericRepository<UserRol>, IUserRolRepository
    {
        private readonly LoginSystemContext _context;

        public UserRolRepository(LoginSystemContext context, ICurrentSessionService currentSessionService) : base(context, currentSessionService) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Rol>> GetRolesByUserId(int userId)
        {
            var roles = await(from ur in _context.UserRols
                              join r in _context.Rols on ur.Id equals r.Id
                              where ur.UserId == userId && r.AuditDeleteDate == null
                              select r).ToListAsync();

            return roles;
        }

        public async Task AssignRolesToUser(int userId, IEnumerable<int> roleIds, int auditUserId)
        {
            await RemoveUserRoles(userId);

            var userRoles = roleIds.Select(roleId => new UserRol
            {
                UserId = userId,
                Id = roleId,
                AuditCreateUser = auditUserId,
                AuditCreateDate = DateTime.UtcNow
            });

            await _context.UserRols.AddRangeAsync(userRoles);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserRoles(int userId)
        {
            var userRoles = await _context.UserRols
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            if (userRoles.Any())
            {
                _context.UserRols.RemoveRange(userRoles);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UserHasRole(int userId, string roleName)
        {
            return await(from ur in _context.UserRols
                         join r in _context.Rols on ur.Id equals r.Id
                         where ur.UserId == userId && r.RolName == roleName && r.AuditDeleteDate == null
                         select r).AnyAsync();
        }

        public async Task<Rol> GetRolByName(string rolName)
        {
            return await _context.Rols
                .FirstOrDefaultAsync(r => r.RolName == rolName && r.AuditDeleteDate == null);
        }
    }
}
