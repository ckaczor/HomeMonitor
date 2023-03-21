using ChrisKaczor.HomeMonitor.Weather.Service.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO.Compression;
using System.Text.Json.Serialization;

namespace ChrisKaczor.HomeMonitor.Weather.Service
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<Database>();

            services.AddHostedService<MessageHandler>();

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:4200")));

            services.AddMvc().AddJsonOptions(configure =>
            {
                configure.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }

        public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
                applicationBuilder.UseDeveloperExceptionPage();

            var database = applicationBuilder.ApplicationServices.GetService<Database>();
            database.EnsureDatabase();

            applicationBuilder.UseCors("CorsPolicy");

            applicationBuilder.UseResponseCompression();

            applicationBuilder.UseRouting();

            applicationBuilder.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}