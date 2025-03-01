using LG.Domain.Entities;
using LG.Infrastructure.Commons.Bases.Request;
using LG.Infrastructure.Commons.Bases.Response;
using LG.Infrastructure.Persistences.Contexts;
using LG.Infrastructure.Persistences.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LG.Infrastructure.Persistences.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly LoginSystemContext _context;

        public UserRepository(LoginSystemContext context) : base(context)
        {
            _context = context;
        }

        
        public async Task<BaseEntityResponse<User>> ListUsers(BaseFiltersRequest filters)
        {
            var response = new BaseEntityResponse<User>();

            var users = GetEntityQuery(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null).AsNoTracking();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        users = users.Where(x => x.Username!.Contains(filters.TextFilter));
                        break;
                    case 2:
                        users = users.Where(x => x.Mail!.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                users = users.Where(x => x.Status!.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                users = users.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) && x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
            }

            if (filters.Sort is null) filters.Sort = "Id";

            response.TotalRecords = await users.CountAsync();
            response.Items = await Ordering(filters, users, true).ToListAsync();

            return response;
        }

        public async Task<bool> EmailExists(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Mail.ToLower() == email.ToLower());
            return user != null;
        }

        public async Task<bool> UsernameExists(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
            return user != null;
        }
    }
}
