using LG.Domain.Commons.Bases.Request;
using LG.Domain.Commons.Bases.Response;
using LG.Domain.Entities;

namespace LG.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<BaseEntityResponse<User>> ListUsers(BaseFiltersRequest filters);
        Task<User> GetUserByIdWithPerson(int userId);
        Task<bool> EmailExists(string email);
        Task<bool> UsernameExists(string username);
        Task<User> AccountByUserName(string userName);
    }
}
