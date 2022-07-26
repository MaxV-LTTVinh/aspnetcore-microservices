using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.API.Persistence;
using Product.API.Repositories;
using Product.API.Repositories.Interfaces;

namespace Product.API.Extensions;

public static class ServiceExtensions
{
    //internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
    //    IConfiguration configuration)
    //{
    //    var jwtSettings = configuration.GetSection(nameof(JwtSettings))
    //        .Get<JwtSettings>();
    //    services.AddSingleton(jwtSettings);

    //    var databaseSettings = configuration.GetSection(nameof(DatabaseSettings))
    //        .Get<DatabaseSettings>();
    //    services.AddSingleton(databaseSettings);

    //    return services;
    //}

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.ConfigureProductDbContext(configuration);
        services.AddInfrastructureServices();
        services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
        // services.AddJwtAuthentication();
        //services.ConfigureHealthChecks();
        return services;
    }

    //internal static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    //{
    //    var settings = services.GetOptions<JwtSettings>(nameof(JwtSettings));
    //    if (settings == null || string.IsNullOrEmpty(settings.Key))
    //        throw new ArgumentNullException($"{nameof(JwtSettings)} is not configured properly");

    //    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));

    //    var tokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateIssuerSigningKey = true,
    //        IssuerSigningKey = signingKey,
    //        ValidateIssuer = false,
    //        ValidateAudience = false,
    //        ValidateLifetime = false,
    //        ClockSkew = TimeSpan.Zero,
    //        RequireExpirationTime = false
    //    };
    //    services.AddAuthentication(o =>
    //    {
    //        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //    }).AddJwtBearer(x =>
    //    {
    //        x.SaveToken = true;
    //        x.RequireHttpsMetadata = false;
    //        x.TokenValidationParameters = tokenValidationParameters;
    //    });

    //    return services;
    //}

    private static IServiceCollection ConfigureProductDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        var builder = new MySqlConnectionStringBuilder(connectionString);

        services.AddDbContext<ProductContext>(m => m.UseMySql(builder.ConnectionString,
            ServerVersion.AutoDetect(builder.ConnectionString), e =>
            {
                e.MigrationsAssembly("Product.API");
                e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            }));

        return services;
    }

    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>))
                .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                .AddScoped<IProductRepository, ProductRepository>()
            ;
    }

    //private static void ConfigureHealthChecks(this IServiceCollection services)
    //{
    //    var databaseSettings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
    //    services.AddHealthChecks()
    //        .AddMySql(databaseSettings.ConnectionString, "MySql Health", HealthStatus.Degraded);
    //}
}