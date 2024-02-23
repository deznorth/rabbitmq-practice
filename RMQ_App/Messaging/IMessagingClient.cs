namespace RMQ_App.Messaging
{
    public interface IMessagingClient
    {
        void SendMessage(string message);
        void SendMessage<T>(T payload);
    }
}
