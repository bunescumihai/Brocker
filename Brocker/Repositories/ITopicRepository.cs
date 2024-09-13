using Brocker.Models;

namespace Brocker.Repositories;

public interface ITopicRepository
{
    List<Topic> GetTopics();

    Topic? ExistsTopic(string name);

}