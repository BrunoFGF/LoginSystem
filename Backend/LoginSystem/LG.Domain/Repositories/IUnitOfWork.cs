namespace LG.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IPersonRepository Person { get; }
        IUserRepository User { get; }
        IUserRolRepository UserRol { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
