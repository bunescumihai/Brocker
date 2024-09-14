using Brocker.Models;

namespace Brocker.Repositories;

public interface IReceiverRepository
{
    List<Article> GetArticles(User user);
    Subscription SubscribeToTopic(int userId, int topicId);
    bool UnsubscribeFromTopic(int userId, int topicId);
    
}