using Blog.Application.Database;
using Blog.Application.Repositories;
using Blog.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPostRepository, PostRepository>();
        services.AddSingleton<IPostService, PostService>();
     
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services,
        string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new NpgsqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        
        return services;
    }
}