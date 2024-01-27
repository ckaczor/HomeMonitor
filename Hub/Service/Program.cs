using ChrisKaczor.Common.OpenTelemetry;
using ChrisKaczor.HomeMonitor.Hub.Service.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace ChrisKaczor.HomeMonitor.Hub.Service;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddCommonOpenTelemetry(Assembly.GetExecutingAssembly().GetName().Name, builder.Configuration["Telemetry:Endpoint"]);

        builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", corsPolicyBuilder => corsPolicyBuilder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()));

        builder.Services.AddSignalR().AddJsonProtocol(options => options.PayloadSerializerOptions.WriteIndented = false);

        // ---

        var app = builder.Build();

        if (builder.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseCors("CorsPolicy");

        app.UseRouting();

        app.MapHub<WeatherHub>("/weather");
        app.MapHub<PowerHub>("/power");
        app.MapHub<DeviceStatusHub>("/device-status");
        app.MapHub<EnvironmentHub>("/environment");

        app.Run();
    }
}