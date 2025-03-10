using LG.Domain.Entities;
using LG.Domain.Repositories;
using LG.Domain.Services;
using LG.Infrastructure.Persistences.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LG.Infrastructure.Persistences.Repositories
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        private readonly LoginSystemContext _context;

        public PersonRepository(LoginSystemContext context, ICurrentSessionService currentSessionService) : base(context, currentSessionService)
        {
            _context = context;
        }

        public async Task<bool> IdentityCardExists(string identityCard)
        {
            return await _context.People.AnyAsync(p => p.IdentityCard == identityCard);
        }
    }
}
