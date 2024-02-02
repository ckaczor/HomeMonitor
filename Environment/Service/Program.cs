using ChrisKaczor.Common.OpenTelemetry;
using ChrisKaczor.HomeMonitor.Environment.Service.Data;
using System.Reflection;

namespace ChrisKaczor.HomeMonitor.Environment.Service;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddCommonOpenTelemetry(Assembly.GetExecutingAssembly().GetName().Name, builder.Configuration["Telemetry:Endpoint"], nameof(MessageHandler));

        builder.Services.AddControllers();

        builder.Services.AddTransient<Database>();
        builder.Services.AddHostedService<MessageHandler>();

        var app = builder.Build();

        app.UseAuthorization();

        app.MapControllers();

        var database = app.Services.GetRequiredService<Database>();
        database.EnsureDatabase();

        app.Run();
    }
}