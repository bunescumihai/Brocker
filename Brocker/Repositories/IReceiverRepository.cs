using Brocker.Models;

namespace Brocker.Repositories;

public interface IReceiverRepository
{
    List<Article> GetArticles(User user);
    Subscription SubscribeToTopic(User user, Topic topic);
    bool UnsubscribeFromTopic(User user, Topic topic);
    
}