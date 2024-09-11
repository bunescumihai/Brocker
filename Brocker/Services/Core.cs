using System.Net.Sockets;
using System.Text;
using Brocker.Exceptions;
using Brocker.Models;
using Newtonsoft.Json;

namespace Brocker.Services;

public static class Core
{
    public static void SubscribeToTopic(Socket socket, Topic topic)
    {
        if (!TopicsManager.ExistsTopic(topic))
            throw new BadTopicException();
        
        Subscriber? subscriber = SubscribersManager.ExistsSubscriber(socket);
        
        if (subscriber is null)
            subscriber = SubscribersManager.AddSubscriber(socket);
        
        subscriber.AddTopic(topic);
    }
    
    public static void UnsubscribeFromTopic(Socket socket, Topic topic)
    {
        if (!TopicsManager.ExistsTopic(topic))
            throw new BadTopicException();
        
        Subscriber? subscriber = SubscribersManager.ExistsSubscriber(socket);
        
        if (subscriber is null)
            subscriber = SubscribersManager.AddSubscriber(socket);
        
        subscriber.RemoveTopic(topic);
    }
    
    public static void SendTopics(Socket socket)
    {
        var sb = new StringBuilder();
        var topics = TopicsManager.Topics;
        
        sb.Append("Available topics:\n");
        
        for(int i = 0; i < topics.Count; i++)
            sb.Append($"{i}. {topics[i].Name}");

        SendResponse<string>(socket, new Response<string>(StatusCode.s200, sb.ToString()));
    }

    static void SendArticleToSubscribers(Article article)
    {
        
    }

    public static void HandleReceivedArticle(Socket socket, Article article)
    {
        SendResponse<string>(socket, new Response<string>(StatusCode.s200, "Your article is received"));
        
        if (!TopicsManager.ExistsTopic(article.Topic))
            throw new BadTopicException();
    }

    public static void SendSimpleResponse(Socket socket, Response<string> stringResponse)
    {
        
    }
    
    private static void SendResponse<T>(Socket socket, Response<T> response)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
        socket.Send(buffer);
    }

}