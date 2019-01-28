using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Test.Web.DataAccess;
using SignalR.Test.Web.NotificationHubs;
using SignalR.Test.Web.ServerNotification;

namespace SignalR.Test.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        protected IGlobalRepository Repository { get; }

        protected NotificationService NotificationService { get; }

        public NotificationController(IGlobalRepository globalRepository, NotificationService notificationService)
        {
            this.Repository = globalRepository;

            this.NotificationService = notificationService;        
        }

        // GET: api/StartNotificationService
        [HttpGet]
        public IActionResult StartNotificationService()
        {
            this.NotificationService.StartNotification();

            return this.Ok();
        }

        // POST: api/Notification
        [HttpPost]
        public IActionResult AddTask(string taskName)
        {
            this.Repository.AddTask(taskName);

            return Ok();
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
