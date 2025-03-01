using LG.Domain.Entities;
using LG.Infrastructure.Commons.Bases.Request;
using LG.Infrastructure.Commons.Bases.Response;

namespace LG.Infrastructure.Persistences.Interfaces
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        Task<BaseEntityResponse<Person>> ListPersons(BaseFiltersRequest filters);
    }
}
