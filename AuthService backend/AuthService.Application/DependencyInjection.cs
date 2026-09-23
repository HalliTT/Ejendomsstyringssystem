using AuthService.Application.Audit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AssemblyMarker).Assembly));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuditBehavior<,>));
            return services;
        }

    }
}
