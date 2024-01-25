using ChrisKaczor.HomeMonitor.Weather.Service.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.IO.Compression;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ChrisKaczor.HomeMonitor.Weather.Service;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddControllers();

        // ---

        var openTelemetry = builder.Services.AddOpenTelemetry();

        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        var serviceName = Assembly.GetExecutingAssembly().GetName().Name;

        openTelemetry.ConfigureResource(resource => resource.AddService(serviceName!));

        openTelemetry.WithMetrics(meterProviderBuilder => meterProviderBuilder
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddProcessInstrumentation()
            .AddMeter("Microsoft.AspNetCore.Hosting")
            .AddMeter("Microsoft.AspNetCore.Server.Kestrel"));

        openTelemetry.WithTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder.AddAspNetCoreInstrumentation(instrumentationOptions => instrumentationOptions.RecordException = true);

            tracerProviderBuilder.AddHttpClientInstrumentation(instrumentationOptions => instrumentationOptions.RecordException = true);

            tracerProviderBuilder.AddSqlClientInstrumentation(o =>
            {
                o.RecordException = true;
                o.SetDbStatementForText = true;
            });

            tracerProviderBuilder.AddSource(nameof(MessageHandler));

            tracerProviderBuilder.SetErrorStatusOnException();

            tracerProviderBuilder.AddOtlpExporter(exporterOptions =>
            {
                exporterOptions.Endpoint = new Uri(builder.Configuration["Telemetry:Endpoint"]!);
                exporterOptions.Protocol = OtlpExportProtocol.Grpc;
            });
        });

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.SetMinimumLevel(LogLevel.Information);
            loggingBuilder.AddOpenTelemetry(options =>
                {
                    options.AddOtlpExporter(exporterOptions =>
                    {
                        exporterOptions.Endpoint = new Uri(builder.Configuration["Telemetry:Endpoint"]!);
                        exporterOptions.Protocol = OtlpExportProtocol.Grpc;
                    });
                }
            );
        });

        builder.Services.AddTransient<Database>();

        builder.Services.AddHostedService<MessageHandler>();

        builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

        builder.Services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
            options.EnableForHttps = true;
        });

        builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", corsPolicyBuilder => corsPolicyBuilder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:4200")));

        builder.Services.AddMvc().AddJsonOptions(configure =>
        {
            configure.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

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