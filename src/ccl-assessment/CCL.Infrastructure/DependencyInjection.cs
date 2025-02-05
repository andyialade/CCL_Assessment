using CCL.Domain.Repositories;
using CCL.Infrastructure.Repostories;
using Microsoft.Extensions.DependencyInjection;

namespace CCL.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IUserDocumentRepository, UserDocumentRepository>();

        return services;
    }
}
