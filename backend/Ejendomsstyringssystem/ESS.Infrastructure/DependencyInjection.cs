using ESS.Application.Properties;
using ESS.Infrastructure.Persistence;
using ESS.Infrastructure.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ESS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<EssDbContext>(options => options.UseNpgsql(config.GetConnectionString("Default")));

            services.AddScoped<IPropertyRepository, PropertyRepository>();

            return services;
        }
    }
}
