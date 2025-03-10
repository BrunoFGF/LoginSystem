using LG.Domain.Entities;

namespace LG.Domain.Repositories
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        Task<bool> IdentityCardExists(string identityCard);
    }
}
