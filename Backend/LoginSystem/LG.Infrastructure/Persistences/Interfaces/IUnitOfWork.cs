namespace LG.Infrastructure.Persistences.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPersonRepository Person { get; }
        IUserRepository User { get; }
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
