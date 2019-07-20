using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ChrisKaczor.HomeMonitor.Weather.Service.Data;

namespace ChrisKaczor.HomeMonitor.Weather.Service
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<Database>();

            services.AddHostedService<MessageHandler>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment environment)
        {
            if (environment.IsDevelopment())
                applicationBuilder.UseDeveloperExceptionPage();

            var database = applicationBuilder.ApplicationServices.GetService<Database>();
            database.EnsureDatabase();

            applicationBuilder.UseMvc();
        }
    }
}