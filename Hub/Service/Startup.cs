using ChrisKaczor.HomeMonitor.Hub.Service.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChrisKaczor.HomeMonitor.Hub.Service
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:4200")));

            services.AddSignalR().AddJsonProtocol(options => options.PayloadSerializerOptions.WriteIndented = false);
        }

        public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }

            applicationBuilder.UseCors("CorsPolicy");

            applicationBuilder.UseRouting();

            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<WeatherHub>("/weather");
                endpoints.MapHub<PowerHub>("/power");
                endpoints.MapHub<DeviceStatusHub>("/device-status");
                endpoints.MapHub<EnvironmentHub>("/environment");
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}