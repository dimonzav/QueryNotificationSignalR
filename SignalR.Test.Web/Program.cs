using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SignalR.Test.Web.ServerNotification;

namespace SignalR.Test.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();

            //NotificationService notificationService = new NotificationService();
            //notificationService.StartNotification();

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
