using LG.Domain.Repositories;
using LG.Domain.Services;
using LG.Infrastructure.Persistences.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace LG.Infrastructure.Persistences.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LoginSystemContext _context;
        private readonly ICurrentSessionService _currentSessionService;
        public IPersonRepository Person { get; set; }
        public IUserRepository User { get; set; }

        public IUserRolRepository UserRol { get; set; }

        public UnitOfWork(LoginSystemContext context, IConfiguration configuration, ICurrentSessionService currentSessionService)
        {
            _context = context;
            Person = new PersonRepository(_context, currentSessionService);
            User = new UserRepository(_context, currentSessionService);
            UserRol = new UserRolRepository(_context, currentSessionService);
            _currentSessionService = currentSessionService;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
