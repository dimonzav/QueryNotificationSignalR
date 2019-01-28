using SignalR.Test.Web.Models;
using SignalR.Test.Web.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SignalR.Test.Web.ServerNotification
{
    public class NotificationService
    {
        private DateTime lastTaskCreationDate = DateTime.Now;
        
        List<Task> Tasks { get; set; }

        protected NotificationHub NotificationHub { get; }

        public NotificationService(NotificationHub notificationHub)
        {
            this.NotificationHub = notificationHub;

            this.Tasks = new List<Task>();
        }

        private bool IsWork { get;  set; }

        private delegate void RateChangeNotification(DataTable table);

        private SqlDependency dependency;

        readonly string ConnectionString = "Data Source=(LocalDb)\\ProjectsV13;Initial Catalog=test_elma;Integrated Security=True;Connection Timeout=90;";

        public void StartNotification()
        {
            SqlDependency.Start(this.ConnectionString, "ChangeTaskAndMessage");
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            connection.Open();

            SqlCommand command = new SqlCommand
            {
                CommandText = "SELECT TaskId, Name, CreationDate FROM dbo.Tasks WHERE CreationDate > @creation_date;",
                Connection = connection,
                CommandType = CommandType.Text
            };

            command.Parameters.AddWithValue("@creation_date", lastTaskCreationDate);

            this.dependency = new SqlDependency(command, "Service=ChangeNotificationService", 60);
            dependency.OnChange += new OnChangeEventHandler(OnItemChange);

            using (SqlDataReader dr = command.ExecuteReader())
            {
                this.Tasks = new List<Task>();

                while (dr.Read())
                {
                    this.Tasks.Add(new Task { TaskId = int.Parse(dr["TaskId"].ToString()), Name = dr["Name"].ToString() });

                    this.lastTaskCreationDate = DateTime.Parse(dr["CreationDate"].ToString());
                }
            }
        }
        private void OnItemChange(object s, SqlNotificationEventArgs e)
        {
            this.StartNotification();

            //this.NotificationHub.SendNotification(this.Tasks[0]);

            System.Threading.Tasks.Task.Run(() => this.NotificationHub.SendNotification(this.Tasks[0]));
        }
        public void StopNotification()
        {
            SqlDependency.Stop(this.ConnectionString, "QueueName");
        }
    }
}