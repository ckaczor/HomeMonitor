using ChrisKaczor.Common.OpenTelemetry;
using ChrisKaczor.HomeMonitor.Power.Service.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO.Compression;
using System.Reflection;

namespace ChrisKaczor.HomeMonitor.Power.Service;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddCommonOpenTelemetry(Assembly.GetExecutingAssembly().GetName().Name, builder.Configuration["Telemetry:Endpoint"], nameof(PowerReader));

        builder.Services.AddControllers();

        builder.Services.AddTransient<Database>();

        builder.Services.AddHostedService<PowerReader>();

        builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

        builder.Services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
            options.EnableForHttps = true;
        });

        builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", corsPolicyBuilder => corsPolicyBuilder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:4200")));

        builder.Services.AddMvc();

        // ---

        var app = builder.Build();

        if (builder.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        var database = app.Services.GetRequiredService<Database>();
        database.EnsureDatabase();

        app.UseCors("CorsPolicy");

        app.UseResponseCompression();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}