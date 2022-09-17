using Common.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Information("Start Ordering API up");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    //builder.Host.AddAppConfigurations();
    //// Add services to the container.
    //builder.Services.AddInfrastructure(builder.Configuration);

    //var app = builder.Build();
    //app.UseInfrastructure();

    //app.MigrateDatabase<ProductContext>().Run();
}

catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information("Shut down Ordering API complete");
    Log.CloseAndFlush();
}