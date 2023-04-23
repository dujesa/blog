using Dapper;

namespace Blog.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync(@"
            create table if not exists posts (
            id UUID primary key,
            slug TEXT not null, 
            title TEXT not null,
            createdat date not null);
        ");
        
        await connection.ExecuteAsync(@"
            create unique index concurrently if not exists posts_slug_idx
            on posts
            using btree(slug);
        ");
            
        await connection.ExecuteAsync(@"
            create table if not exists categories (
            postId UUID references posts (Id),
            name TEXT not null);
        ");

        await connection.ExecuteAsync(@"
            create table if not exists ratings (
                userid uuid,
                postid uuid references posts (id),
                rating integer not null,
                primary key (userid, postid)
            );
        ");
    }
}