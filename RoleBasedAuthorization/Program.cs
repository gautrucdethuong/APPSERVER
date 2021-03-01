using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace RoleBasedAuthorization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            //var host = new WebHostBuilder()
            // .UseKestrel()
            // .UseContentRoot(Directory.GetCurrentDirectory())
            // //.UseUrls("http://localhost:5000")
            // .UseIISIntegration()
            // .UseStartup<Startup>()
            // .Build();

            //host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
