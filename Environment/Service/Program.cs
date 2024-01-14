using ChrisKaczor.HomeMonitor.Environment.Service.Data;

namespace ChrisKaczor.HomeMonitor.Environment.Service;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddControllers();

        builder.Services.AddTransient<Database>();
        builder.Services.AddHostedService<MessageHandler>();

        var app = builder.Build();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}