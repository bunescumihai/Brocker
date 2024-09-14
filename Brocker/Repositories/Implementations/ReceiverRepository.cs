using Brocker.DbContexts;
using Brocker.Models;

namespace Brocker.Repositories.Implementations;

public class ReceiverRepository : IReceiverRepository
{
    private BrockerDbContext _dbContext = BrockerDbContext.GetBrockerDbContext();
    
    public List<Article> GetArticles(User user)
    {
        throw new NotImplementedException();
    }

    public Subscription SubscribeToTopic(int userId, int topicId)
    {
        var sub = ExistsSubscription(new Subscription(userId, topicId));
        
        if (sub != null)
            return sub;
        
        sub = new Subscription() { TopicId = topicId, UserId = userId };
        
        _dbContext.Subscriptions.Add(sub);
        _dbContext.SaveChanges();
        
        return sub;
    }

    private Subscription ExistsSubscription(Subscription subscription)
    {
        var sub = _dbContext.Subscriptions.FirstOrDefault(sub =>
            (sub.TopicId == subscription.TopicId) && (sub.UserId == subscription.UserId));

        return sub;
    }

    public bool UnsubscribeFromTopic(int userId, int topicId)
    {
        var sub = _dbContext.Subscriptions.FirstOrDefault(sub => (sub.TopicId == topicId && sub.UserId == userId));
        
        if (sub is null)
            return true;
        
        _dbContext.Subscriptions.Remove(sub);
        _dbContext.SaveChanges();
        
        return true;
    }
}