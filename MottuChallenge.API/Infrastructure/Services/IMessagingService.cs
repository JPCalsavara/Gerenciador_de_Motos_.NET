namespace MottuChallenge.API.Services
{
    public interface IMessagingService
    {
        void Publish(string queue, object message);
    }
}