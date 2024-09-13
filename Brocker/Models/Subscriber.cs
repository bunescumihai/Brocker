using System.Net.Sockets;

namespace Brocker.Models;

public class Subscriber
{
    public Socket Socket { get; }
    public List<Topic> Topics { get; } = new List<Topic>();

    public Subscriber(Socket socket)
    {
        Socket = socket;
    }

    public void AddTopic(Topic topic)
    {
        foreach (var tp in Topics)
            if (tp.Name.Equals(topic.Name))
                return;
        
        Topics.Add(topic);
    }
    
    
    public void RemoveTopic(Topic topic)
    {
        foreach (var tp in Topics)
            if (tp.Name.Equals(topic.Name))
            {
                Topics.Remove(tp);
                return;
            }
    }
}