
namespace Contracts.Messages
{
    public interface IMessagePublisher
    {
        void SendMessage<T>(T message);
    }
}
