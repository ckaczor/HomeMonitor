using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ChrisKaczor.HomeMonitor.Hub.Service
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }
    }
}