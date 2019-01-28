using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Test.Web.NotificationHubs;
using SignalR.Test.Web.DataAccess;
using SignalR.Test.Web.ServerNotification;

namespace SignalR.Test.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        protected IGlobalRepository Repository { get; }

        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationController(IHubContext<NotificationHub> hubContext, IGlobalRepository globalRepository)
        {
            NotificationService notificationService = new NotificationService();
            notificationService.StartNotification();

            this.Repository = globalRepository;

            _hubContext = hubContext;
        }

        // POST: api/Notification
        [HttpPost]
        public IEnumerable<string> AddTask(string taskName)
        {
            this.Repository.AddTask(taskName);

            //await _hubContext.Clients.All.SendAsync("ReceiveTask", task);

            return new string[] { "value1", "value2" };
        }

        // POST: api/Notification
        [HttpPost]
        [Route("SendMessage")]
        public IEnumerable<string> SendMessage(string userName, string messageText)
        {
            this.Repository.AddMessage(userName, messageText);

            //await _hubContext.Clients.All.SendAsync("ReceiveTask", task);

            return new string[] { "value1", "value2" };
        }
    }
}
