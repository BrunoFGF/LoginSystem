using LG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LG.Infrastructure.Persistences.Contexts;

public partial class LoginSystemContext : DbContext
{
    public LoginSystemContext()
    {
    }

    public LoginSystemContext(DbContextOptions<LoginSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<RolOption> RolOptions { get; set; }

    public virtual DbSet<RolRolOption> RolRolOptions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRol> UserRols { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
