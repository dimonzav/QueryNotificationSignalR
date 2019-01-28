using SignalR.Test.Web.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SignalR.Test.Web.ServerNotification
{
    public class NotificationService
    {
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
                CommandText = "SELECT TaskId, Name FROM dbo.Tasks;",
                Connection = connection,
                CommandType = CommandType.Text
            };

            this.dependency = new SqlDependency(command, "Service=ChangeNotificationService", 60);
            dependency.OnChange += new OnChangeEventHandler(OnItemChange);

            using (SqlDataReader dr = command.ExecuteReader())
            {
                List<Task> tasks = new List<Task>();

                while (dr.Read())
                {
                    tasks.Add(new Task { TaskId = int.Parse(dr["TaskId"].ToString()), Name = dr["Name"].ToString() });
                }
            }


        }
        private void OnItemChange(object s, SqlNotificationEventArgs e)
        {
            IsWork = true;
        }
        public void StopNotification()
        {
            SqlDependency.Stop(this.ConnectionString, "QueueName");
        }
    }
}