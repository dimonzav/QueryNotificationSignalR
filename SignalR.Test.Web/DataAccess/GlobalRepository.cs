﻿using System;
using System.Data.SqlClient;

namespace SignalR.Test.Web.DataAccess
{
    public class GlobalRepository : IGlobalRepository
    {
        private readonly string ConnectionString;

        public GlobalRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public void AddTask(string taskName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    string query = "INSERT INTO dbo.Tasks (Name, CreationDate) VALUES (@name, @creation_date)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", taskName);
                        command.Parameters.AddWithValue("@creation_date", DateTime.Now);

                        connection.Open();

                        int result = command.ExecuteNonQuery();

                        if (result < 0)
                        {
                            throw new Exception("Error adding task to database.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddMessage(string userName, string messageText)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    string query = "INSERT INTO dbo.Messages (UserName, MessageText, CreationDate) VALUES (@user_name, @message_text, @creation_date)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@user_name", userName);
                        command.Parameters.AddWithValue("@message_text", messageText);
                        command.Parameters.AddWithValue("@creation_date", DateTime.Now);

                        connection.Open();

                        int result = command.ExecuteNonQuery();

                        if (result < 0)
                        {
                            throw new Exception("Error adding message to database.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
