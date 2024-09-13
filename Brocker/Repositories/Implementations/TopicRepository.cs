using Brocker.DbContexts;
using Brocker.Models;

namespace Brocker.Repositories.Implementations;

public class TopicRepository : ITopicRepository
{
    private BrockerDbContext _dbContext = BrockerDbContext.GetBrockerDbContext();

    public List<Topic> GetTopics() => _dbContext.Topics.ToList();

    public Topic? ExistsTopic(string name) => _dbContext.Topics.First(topic => topic.Name.Equals(name));
}