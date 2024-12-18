using ChrisKaczor.Common.OpenTelemetry;
using System.Reflection;

namespace ChrisKaczor.HomeMonitor.Calendar.Service;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", corsPolicyBuilder => corsPolicyBuilder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:4200")));

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddCommonOpenTelemetry(Assembly.GetExecutingAssembly().GetName().Name, builder.Configuration["Telemetry:Endpoint"]);

        builder.Services.AddSingleton<HttpClient>();

        builder.Services.AddControllers();

        // -- -- 

        var app = builder.Build();

        app.UseCors("CorsPolicy");

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}