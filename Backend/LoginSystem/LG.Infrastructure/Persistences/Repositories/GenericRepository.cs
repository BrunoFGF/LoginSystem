using LG.Domain.Entities;
using LG.Infrastructure.Commons.Bases.Request;
using LG.Infrastructure.Helpers;
using LG.Infrastructure.Persistences.Contexts;
using LG.Infrastructure.Persistences.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace LG.Infrastructure.Persistences.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly LoginSystemContext _context;
        private readonly DbSet<T> _entity;

        public GenericRepository(LoginSystemContext context)
        {
            _context = context;
            _entity = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var getAll = await _entity
                .Where(x =>  x.AuditDeleteUser == null && x.AuditDeleteDate == null).AsNoTracking().ToListAsync();

            return getAll;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var getById = await _entity!
                .Where(x =>  x.AuditDeleteUser == null && x.AuditDeleteDate == null)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            return getById!;
        }

        public async Task<bool> RegisterAsync(T entity)
        {
            entity.AuditCreateUser = 1;
            entity.AuditCreateDate = DateTime.Now;


            await _context.AddAsync(entity);

            var recordsAffected = await _context.SaveChangesAsync();

            return recordsAffected > 0;
        }

        public async Task<bool> EditAsync(T entity)
        {
            entity.AuditUpdateUser = 1;
            entity.AuditUpdateDate = DateTime.Now;

            _context.Update(entity);
            _context.Entry(entity).Property(x => x.AuditCreateUser).IsModified = false;
            _context.Entry(entity).Property(x => x.AuditCreateDate).IsModified = false;

            var recordsAffected = await _context.SaveChangesAsync();

            return recordsAffected > 0;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            T entity = await GetByIdAsync(id);

            entity!.AuditDeleteUser = 1;
            entity.AuditDeleteDate = DateTime.Now;

            _context.Update(entity);

            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }

        public IQueryable<T> GetEntityQuery(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _entity;

            if (filter != null) query = query.Where(filter);

            return query;
        }

        public IQueryable<TDTO> Ordering<TDTO>(BasePaginationRequest request, IQueryable<TDTO> queryable, bool pagination = false) where TDTO : class
        {
            IQueryable<TDTO> queryDto = request.Order == "desc" ? queryable.OrderBy($"{request.Sort} descending") : queryable.OrderBy($"{request.Sort} ascending");

            if (pagination) queryDto = queryDto.Paginate(request);

            return queryDto;
        }
    }
}
