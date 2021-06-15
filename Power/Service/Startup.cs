using ChrisKaczor.HomeMonitor.Power.Service.Data;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO.Compression;
using System.Threading;

namespace ChrisKaczor.HomeMonitor.Power.Service
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();

            services.AddApplicationInsightsTelemetry(options =>
            {
                options.EnableDependencyTrackingTelemetryModule = false;
            });

            services.AddTransient<Database>();

            services.AddHostedService<PowerReader>();

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:4200")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment environment, IHostApplicationLifetime hostApplicationLifetime)
        {
            if (environment.IsDevelopment())
                applicationBuilder.UseDeveloperExceptionPage();

            hostApplicationLifetime.ApplicationStopping.Register(() =>
            {
                var telemetryClient = applicationBuilder.ApplicationServices.GetRequiredService<TelemetryClient>();

                telemetryClient.Flush();

                Thread.Sleep(5000);
            });

            var database = applicationBuilder.ApplicationServices.GetRequiredService<Database>();
            database.EnsureDatabase();

            applicationBuilder.UseCors("CorsPolicy");

            applicationBuilder.UseResponseCompression();

            applicationBuilder.UseRouting();

            applicationBuilder.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}