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
    private static IReceiverRepository _receiverRepository = new ReceiverRepository();
    
    public static void SubscribeToTopic(Socket socket, Credentials credentials, Topic topic)
    {
        var user = _userRepository.GetUser(credentials.UserName, credentials.Password);
        
        if (user is null)
            throw new UnauthorizedException();
        
        if (user.UserRole != UserRole.Receiver)
            throw new PermissionException();
        
        var tp = _topicRepository.ExistsTopic(topic.Name);
        
        if (topic is null)
            throw new BadTopicException();

        _receiverRepository.SubscribeToTopic((int)user.Id!, (int)tp.Id!);
        
        SendResponse<string>(socket, new Response<string>(StatusCode.s200, $"You were subscribed to topic {tp.Name}"));
    }
    
    public static void UnsubscribeFromTopic(Socket socket, Credentials credentials, Topic topic)
    {
        var user = _userRepository.GetUser(credentials.UserName, credentials.Password);
        
        if (user is null)
            throw new UnauthorizedException();
        
        if (user.UserRole != UserRole.Receiver)
            throw new PermissionException();
        
        var tp = _topicRepository.ExistsTopic(topic.Name);
        
        if (topic is null)
            throw new BadTopicException();

        _receiverRepository.UnsubscribeFromTopic((int)user.Id!, (int)tp.Id!);
        
        SendResponse<string>(socket, new Response<string>(StatusCode.s200, $"You were unsubscribed from topic {tp.Name}"));
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

    public static void SendTopics(Socket socket)
    {
        var topics = _topicRepository.GetTopics();

        SendResponse<List<Topic>>(socket, new Response<List<Topic>>(StatusCode.s200, topics));
    }

    public static void HandleReceivedArticle(Socket socket,Credentials credentials,  Article article)
    {
        var user = _userRepository.GetUser(credentials.UserName, credentials.Password);
        
        if (user is null)
            throw new UnauthorizedException();

        var topic = _topicRepository.ExistsTopic(article.Topic.Name);
        
        if (topic is null)
            throw new BadTopicException();

        _articleRepository.CreateArticle(new Article((int)user.Id!, (int)topic.Id!, article.Content));
        
        SendResponse<string>(socket, new Response<string>(StatusCode.s200, "Your article was received"));
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