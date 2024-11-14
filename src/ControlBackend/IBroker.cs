// The ControlBackend namespace is defined
namespace ControlBackend
{
    // The IBroker interface is defined within the ControlBackend namespace
    public interface IBroker
    {
        // This method is used to publish a message to all subscribers of a topic
        // It takes a topic string and a message as a byte array
        void Publish(string topic, byte[] message);

        // This method is used to subscribe to a topic
        // It takes a topic string and an ISubscriber object as arguments
        // The method returns a string tag that can be used to cancel the subscription
        string Subscribe(string topic, ISubscriber sub);

        // This method is used to cancel a subscription
        // It takes a tag string that was returned by the Subscribe method
        void CancelRecv(string tag);
    }
}
