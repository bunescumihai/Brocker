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
        
        SendResponse<string>(socket, new Response<string>(StatusCode.s200, $"You were subscribed to {topic.Name}"));
    }
    
    public static void UnsubscribeFromTopic(Socket socket, Topic topic)
    {
        if (!TopicsManager.ExistsTopic(topic))
            throw new BadTopicException();
        
        Subscriber? subscriber = SubscribersManager.ExistsSubscriber(socket);
        
        if (subscriber is null)
            subscriber = SubscribersManager.AddSubscriber(socket);
        
        subscriber.RemoveTopic(topic);
        
        SendResponse<string>(socket, new Response<string>(StatusCode.s200, $"You were unsubscribed from {topic.Name}"));
    }
    
    public static void SendTopics(Socket socket)
    {
        var sb = new StringBuilder();
        var topics = TopicsManager.Topics;

        sb.Append("Available topics:\n");
        
        for(int i = 0; i < topics.Count; i++)
            sb.Append($"{i}. {topics[i].Name}");

        SendResponse<List<Topic>>(socket, new Response<List<Topic>>(StatusCode.s200, topics));
    }

    static void SendArticleToSubscribers(Article article)
    {
        List<Subscriber> subscribers = SubscribersManager.GetTopicSubscribers(article.Topic);

        foreach (var sub in subscribers)
            try
            {
                SendResponse<Article>(sub.Socket, new Response<Article>(StatusCode.s200, article));
            }
            catch (Exception e)
            {
                
            }
    }

    public static void HandleReceivedArticle(Socket socket, Article article)
    {
        SendResponse<string>(socket, new Response<string>(StatusCode.s200, "Your article is received"));
        
        if (!TopicsManager.ExistsTopic(article.Topic))
            throw new BadTopicException();
        
        SendArticleToSubscribers(article);
    }

    public static void SendSimpleResponse(Socket socket, Response<string> stringResponse)
    {
        try
        {
            SendResponse(socket, stringResponse);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private static void SendResponse<T>(Socket socket, Response<T> response)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
        socket.Send(buffer);
    }

}