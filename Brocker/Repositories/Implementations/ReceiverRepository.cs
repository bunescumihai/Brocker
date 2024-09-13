using Brocker.Models;

namespace Brocker.Repositories.Implementations;

public class ReceiverRepository : IReceiverRepository
{
    public List<Article> GetArticles(User user)
    {
        throw new NotImplementedException();
    }

    public Subscription SubscribeToTopic(User user, Topic topic)
    {
        throw new NotImplementedException();
    }

    public bool UnsubscribeFromTopic(User user, Topic topic)
    {
        throw new NotImplementedException();
    }
}