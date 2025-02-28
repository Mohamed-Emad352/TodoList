using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Infrastructure.Interceptors;

namespace TodoList.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
    }
}