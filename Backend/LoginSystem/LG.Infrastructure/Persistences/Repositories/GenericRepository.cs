using LG.Domain.Commons.Bases.Request;
using LG.Domain.Entities;
using LG.Domain.Repositories;
using LG.Domain.Services;
using LG.Infrastructure.Helpers;
using LG.Infrastructure.Persistences.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace LG.Infrastructure.Persistences.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly LoginSystemContext _context;
        private readonly DbSet<T> _entity;
        private readonly ICurrentSessionService _sessionService;

        public GenericRepository(LoginSystemContext context, ICurrentSessionService sessionService)
        {
            _context = context;
            _entity = _context.Set<T>();
            _sessionService = sessionService;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var getAll = await _entity
                .Where(x =>  x.AuditDeleteUser == null && x.AuditDeleteDate == null).AsNoTracking().ToListAsync();

            return getAll;
        }

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _entity!.Where(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null);

            // Aplicar los includes
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var getById = await query.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
            return getById!;
        }

        public async Task<bool> RegisterAsync(T entity)
        {
            var userId = _sessionService.Get();

            entity.AuditCreateUser = userId;
            entity.AuditCreateDate = DateTime.Now;


            await _context.AddAsync(entity);

            var recordsAffected = await _context.SaveChangesAsync();

            return recordsAffected > 0;
        }

        public async Task<bool> EditAsync(T entity)
        {
            var userId = _sessionService.Get();

            entity.AuditUpdateUser = userId;
            entity.AuditUpdateDate = DateTime.Now;

            var existingEntity = await _entity.FindAsync(entity.Id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Update(entity);
            _context.Entry(entity).Property(x => x.AuditCreateUser).IsModified = false;
            _context.Entry(entity).Property(x => x.AuditCreateDate).IsModified = false;

            var recordsAffected = await _context.SaveChangesAsync();

            return recordsAffected > 0;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var userId = _sessionService.Get();

            T entity = await GetByIdAsync(id);

            entity!.AuditDeleteUser = userId;
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
