using ESS.Application.Properties;
using ESS.Application.Units;
using ESS.Infrastructure.Persistence;
using ESS.Infrastructure.Properties;
using ESS.Infrastructure.Units;
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
            services.AddScoped<IUnitRepository, UnitsRepository>();

            return services;
        }
    }
}
