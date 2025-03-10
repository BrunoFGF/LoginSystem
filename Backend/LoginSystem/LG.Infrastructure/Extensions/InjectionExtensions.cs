using LG.Domain.Repositories;
using LG.Domain.Services;
using LG.Infrastructure.Persistences.Contexts;
using LG.Infrastructure.Persistences.Repositories;
using LG.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LG.Infrastructure.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(LoginSystemContext).Assembly.FullName;

            services.AddDbContext<LoginSystemContext>(
                options => options.UseSqlServer(
                    configuration.GetConnectionString("LGConnection"), b => b.MigrationsAssembly(assembly)), ServiceLifetime.Transient);

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICurrentSessionService, CurrentSessionService>();

            return services;
        }
    }
}