using FluentValidation.AspNetCore;
using LG.Application.Interfaces;
using LG.Application.Services;
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

            services.AddScoped<IPersonApplication, PersonApplication>();
            services.AddScoped<IUserApplication, UserApplication>();

            return services;
        }
    }
}

