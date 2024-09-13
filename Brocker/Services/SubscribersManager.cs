using System.Net.Sockets;
using Brocker.Models;

namespace Brocker.Services;

public static class SubscribersManager
{
    public static List<Subscriber> Subscribers = new List<Subscriber>();

    public static Subscriber? ExistsSubscriber(Socket socket)
    {
        foreach (var sb in Subscribers)
            if (sb.Socket.Equals(socket))
                return sb;
        
        return null;
    }

    public static Subscriber AddSubscriber(Socket socket)
    {
        Subscriber subscriber = new Subscriber(socket);
        Subscribers.Add(subscriber);
        return subscriber;
    }
    
    public static void RemoveSubscriber(Socket socket)
    {
        Subscriber? subscriber = null;
        
        foreach (var sb in Subscribers)
            if (sb.Socket.Equals(socket))
            {
                subscriber = sb;
            }

        if (subscriber is not null)
            Subscribers.Remove(subscriber);
    }

    public static List<Subscriber> GetTopicSubscribers(Topic topic)
    {
        List<Subscriber> list = new List<Subscriber>();
        
        foreach (var sub in Subscribers)
            foreach (var tp in sub.Topics)
                if (tp.Name.Equals(topic.Name))
                    {
                        list.Add(sub);
                        break;
                    }  
        
        return list;
    }
    
}