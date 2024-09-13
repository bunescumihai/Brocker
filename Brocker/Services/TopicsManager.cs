using System.Runtime.CompilerServices;
using Brocker.Models;

namespace Brocker.Services;

public static class TopicsManager
{
    public static List<Topic> Topics { get; } = new List<Topic> { new Topic(), new Topic() };

    public static bool ExistsTopic(Topic topic)
    {
        foreach (var tp in Topics)
            if (tp.Name.ToUpper().Equals(topic.Name.ToUpper()))
                return true;
        return false;
    }
}