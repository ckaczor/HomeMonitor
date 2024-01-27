using ChrisKaczor.Common.OpenTelemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO.Compression;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ChrisKaczor.HomeMonitor.Weather.SerialReader;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddCommonOpenTelemetry(Assembly.GetExecutingAssembly().GetName().Name, builder.Configuration["Telemetry:Endpoint"], nameof(SerialReader));

        builder.Services.AddControllers();

        builder.Services.AddHostedService<SerialReader>();

        builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

        builder.Services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
            options.EnableForHttps = true;
        });

        builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", corsPolicyBuilder => corsPolicyBuilder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:4200")));

        builder.Services.AddMvc().AddJsonOptions(configure => configure.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        // ---

        var app = builder.Build();

        if (builder.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseCors("CorsPolicy");

        app.UseResponseCompression();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}