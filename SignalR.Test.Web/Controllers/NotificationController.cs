using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SignalR.Test.Web.DataAccess;
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
        public IActionResult SendMessage(string userName, string messageText)
        {
            this.Repository.AddMessage(userName, messageText);

            return Ok();
        }




        [HttpGet]
        [Route("GetFile")]
        public async System.Threading.Tasks.Task<IActionResult> GetFileAsync()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://185.59.101.152:8080/PublicAPI/REST/EleWise.ELMA.Messages/MessageFeed/Posts/Feed?limit=5");

            request.Headers.Accept.Clear();
            request.Headers.Add("AuthToken", "6ad2160c-5107-401d-b033-6d1b0ecff9fe");

            HttpClient client = new HttpClient();

            using (HttpResponseMessage response = await client.SendAsync(request))
            {
                try
                {
                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();

                    dynamic json = JObject.Parse(result);

                    string actionObjectId = json.Data[0].ChangeDate;

                    dynamic resultJson = new List<dynamic>();

                    foreach (var item in json.Data)
                    {
                        if (item.ChangeDate >= DateTime.Now.AddDays(-1))
                        {
                            resultJson.Add(item);
                        }
                    }

                    return Ok();
                }
                catch (Exception ex)
                {
                    throw new Exception($"There was some error occurred: {ex.Message}");
                }
            }
        }
    }
}
