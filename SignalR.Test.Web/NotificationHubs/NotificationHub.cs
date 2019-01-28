namespace SignalR.Test.Web.NotificationHubs
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    public class NotificationHub : Hub
    {
        public async Task SendNotification(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", user, message);
        }
    }
}
