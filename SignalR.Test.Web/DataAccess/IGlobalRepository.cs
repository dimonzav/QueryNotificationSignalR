namespace SignalR.Test.Web.DataAccess
{
    public interface IGlobalRepository
    {
        void AddTask(string taskName);

        void AddMessage(string userName, string messageText);
    }
}
