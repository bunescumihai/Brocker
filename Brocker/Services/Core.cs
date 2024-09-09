using System.Net.Sockets;
using System.Text;
using Brocker.Models;

namespace Brocker.Services;

public static class Core
{
    static List<Topic> topics = new List<Topic>{new Topic("Music"), new Topic("Football")};
    static List<Subscriber> subscribers = new List<Subscriber>();

    static void SubscribeToTopic(Socket socket, Topic topic)
    {
        
    }
    
    static void UnsubscribeFromTopic(Socket socket, Topic topic)
    {
        
    }
    
    static void SendTopics(Socket socket)
    {
        
    }

    static void SendArticleToSubscriber(Article article)
    {
        
    }

    static void SendBadResponse(Socket socket, string message)
    {
        
    }

    private static void Send(Socket socket, string response)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(response);
        socket.Send(buffer);
    }
    
}