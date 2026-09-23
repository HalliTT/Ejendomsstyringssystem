using ESS.Application.Common.Interface;
using ESS.Application.Properties;
using ESS.Application.Rentals;
using ESS.Application.Tenants;
using ESS.Application.Units;
using ESS.Application.Users;
using ESS.Infrastructure.Common;
using ESS.Infrastructure.Persistence;
using ESS.Infrastructure.Properties;
using ESS.Infrastructure.Rentals;
using ESS.Infrastructure.Tenants;
using ESS.Infrastructure.Units;
using ESS.Infrastructure.Users;
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
            services.AddScoped<IUserProvisioningService, UserProvisioningService>();
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IRentalOptionRepository, RentalOptionRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();

            return services;
        }
    }
}
