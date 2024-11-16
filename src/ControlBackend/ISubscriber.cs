// The ControlBackend namespace is defined
namespace ControlBackend
{
    // The ISubscriber interface is defined within the ControlBackend namespace
    public interface ISubscriber
    {
        // This method is called when a message is received by a subscriber
        // It takes a topic string and a message as a byte array
        void OnMessage(string topic, byte[] body);
    }
}
