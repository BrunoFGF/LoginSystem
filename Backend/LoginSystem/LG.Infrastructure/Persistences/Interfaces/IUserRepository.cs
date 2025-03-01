using LG.Domain.Entities;
using LG.Infrastructure.Commons.Bases.Request;
using LG.Infrastructure.Commons.Bases.Response;

namespace LG.Infrastructure.Persistences.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<BaseEntityResponse<User>> ListUsers(BaseFiltersRequest filters);
        Task<bool> EmailExists(string email);
        Task<bool> UsernameExists(string username);
    }
}
