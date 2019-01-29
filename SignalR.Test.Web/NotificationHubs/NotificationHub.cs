namespace SignalR.Test.Web.NotificationHubs
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    public class NotificationHub : Hub
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationHub(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotification(Models.Task task)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveTask", $"Task {task.Name} was created.");
        }
    }
}
