using FluentValidation.AspNetCore;
using LG.Application.Interfaces;
using LG.Application.Services;
using LG.Domain.Services;
using LG.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LG.Application.Extensions
{
    public static class InjectionExtension
    {
        public static IServiceCollection AddInjectionApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            services.AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic));
            });
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IUserApplication, UserApplication>();

            services.AddScoped<IRoleApplication, RoleApplication>();
            services.AddScoped<IAccountApplication, AccountApplication>();
            services.AddScoped<ICurrentSessionService, CurrentSessionService>();

            return services;
        }
    }
}

