namespace ControlBackend.Interfaces
{
    public interface ISubscriber
    {
        void OnMessage(string topic, byte[] body);
    }

    public interface IBroker
    {
        void Publish(string topic, byte[] message);

        string Subscribe(string topic, ISubscriber sub);

        void CancelRecv(string tag);
    }
}
