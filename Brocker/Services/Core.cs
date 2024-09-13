using System.Net.Sockets;
using System.Text;
using Brocker.Exceptions;
using Brocker.Models;
using Brocker.Repositories;
using Brocker.Repositories.Implementations;
using Newtonsoft.Json;

namespace Brocker.Services;

public static class Core
{
    private static ITopicRepository _topicRepository = new TopicRepository();
    private static IUserRepository _userRepository = new UserRepository();
    private static IArticleRepository _articleRepository = new ArticleRepository();
    
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
    
    public static void RegisterAsSender(Socket socket, User user)
    {
        var usr = _userRepository.RegisterLikeASender(user.UserName, user.Password);
        
        SendResponse<User>(socket, new Response<User>(StatusCode.s200, user));
    }
    
    public static void RegisterAsReceiver(Socket socket, User user)
    {
        var usr = _userRepository.RegisterLikeAReceiver(user.UserName, user.Password);
        
        SendResponse<User>(socket, new Response<User>(StatusCode.s200, user));
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
        Console.WriteLine("From Send Topics");
        var topics = _topicRepository.GetTopics();

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

    public static void HandleReceivedArticle(Socket socket,ICredentials credentials,  Article article)
    {
        var user = _userRepository.GetUser(credentials.UserName, credentials.Password);
        
        if (user is null)
            throw new UnauthorizedException();
        
        SendResponse<string>(socket, new Response<string>(StatusCode.s200, "Your article is received"));

        var topic = _topicRepository.ExistsTopic(article.Topic.Name);
        
        if (topic is null)
            throw new BadTopicException();
        
        var art = new Article();
        
        
            
        _articleRepository.CreateArticle(new Article());
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