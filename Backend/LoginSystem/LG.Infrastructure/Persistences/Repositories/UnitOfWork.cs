using LG.Infrastructure.Persistences.Contexts;
using LG.Infrastructure.Persistences.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LG.Infrastructure.Persistences.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LoginSystemContext _context;
        public IPersonRepository Person { get; set; }
        public IUserRepository User { get; set; }

        public UnitOfWork(LoginSystemContext context, IConfiguration configuration)
        {
            _context = context;
            Person = new PersonRepository(_context);
            User = new UserRepository(_context);
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
