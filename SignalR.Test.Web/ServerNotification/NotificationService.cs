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
        private readonly string ConnectionString;

        private DateTime lastTaskCreationDate = DateTime.Now;
        
        List<Task> Tasks { get; set; }

        protected NotificationHub NotificationHub { get; }

        public NotificationService(NotificationHub notificationHub, string connectionString)
        {
            this.ConnectionString = connectionString;

            this.NotificationHub = notificationHub;

            this.Tasks = new List<Task>();
        }

        private bool IsWork { get;  set; }

        private SqlDependency dependency;

        public void StartNotification()
        {
            SqlDependency.Start(this.ConnectionString, "ChangeTaskAndMessage");
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            connection.Open();

            SqlCommand command = new SqlCommand
            {
                CommandText = "SELECT TaskId, Name, LastDate " +
                "FROM dbo.Tasks tasks " +
                "INNER JOIN(SELECT CreationDate, MAX(CreationDate) as LastDate " +
                "FROM dbo.Tasks " +
                "WHERE CreationDate > @lastCreationDate" +
                "GROUP BY CreationDate) as tasks2 " +
                "ON tasks.CreationDate = tasks2.LastDate; ",
                Connection = connection,
                CommandType = CommandType.Text
            };

            command.Parameters.AddWithValue("@lastCreationDate", lastTaskCreationDate);

            this.dependency = new SqlDependency(command, "Service=ChangeNotificationService", 60);
            dependency.OnChange += new OnChangeEventHandler(OnItemChange);

            using (SqlDataReader dr = command.ExecuteReader())
            {
                this.Tasks = new List<Task>();

                while (dr.Read())
                {
                    this.Tasks.Add(new Task { TaskId = int.Parse(dr["TaskId"].ToString()), Name = dr["Name"].ToString() });

                    this.lastTaskCreationDate = DateTime.Parse(dr["LastDate"].ToString());
                }
            }
        }
        private void OnItemChange(object s, SqlNotificationEventArgs e)
        {
            this.StartNotification();

            System.Threading.Tasks.Task.Run(() => this.NotificationHub.SendNotification(this.Tasks[0]));
        }
        public void StopNotification()
        {
            SqlDependency.Stop(this.ConnectionString, "ChangeTaskAndMessage");
        }
    }
}