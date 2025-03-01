using LG.Domain.Entities;
using LG.Infrastructure.Commons.Bases.Request;
using LG.Infrastructure.Commons.Bases.Response;
using LG.Infrastructure.Persistences.Contexts;
using LG.Infrastructure.Persistences.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LG.Infrastructure.Persistences.Repositories
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(LoginSystemContext context): base(context) { }

        public async Task<BaseEntityResponse<Person>> ListPersons(BaseFiltersRequest filters)
        {
            var response = new BaseEntityResponse<Person>();

            var persons = GetEntityQuery(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null).AsNoTracking();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        persons = persons.Where(x => x.FirstName!.Contains(filters.TextFilter));
                        break;
                    case 2:
                        persons = persons.Where(x => x.LastName!.Contains(filters.TextFilter));
                        break;
                }
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                persons = persons.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) && x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
            }

            if (filters.Sort is null) filters.Sort = "Id";

            response.TotalRecords = await persons.CountAsync();
            response.Items = await Ordering(filters, persons, true).ToListAsync();

            return response;
        }
    }
}
