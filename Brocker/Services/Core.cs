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
    
    public static Response SubscribeToTopic(Credentials credentials, Topic topic)
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

        return new Response(StatusCode.s200, $"You were subscribed to topic {tp.Name}");
    }
    
    public static Response UnsubscribeFromTopic(Credentials credentials, Topic topic)
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

        return new Response(StatusCode.s200, $"You were unsubscribed from topic {tp.Name}");
    }
    
    public static Response RegisterAsSender(User user)
    {
        var usr = _userRepository.RegisterLikeASender(user.UserName, user.Password);
        
        return new Response(StatusCode.s200, user);
    }
    
    public static Response RegisterAsReceiver(User user)
    {
        var usr = _userRepository.RegisterLikeAReceiver(user.UserName, user.Password);

        return new Response(StatusCode.s200, user);
    }

    public static  Response GetTopics()
    {
        var topics = _topicRepository.GetTopics();

        return new Response(StatusCode.s200, topics);
    }

    public static Response HandleReceivedArticle(Credentials credentials,  Article article)
    {
        var user = _userRepository.GetUser(credentials.UserName, credentials.Password);
        
        if (user is null)
            throw new UnauthorizedException();

        var topic = _topicRepository.ExistsTopic(article.Topic.Name);
        
        if (topic is null)
            throw new BadTopicException();

        _articleRepository.CreateArticle(new Article((int)user.Id!, (int)topic.Id!, article.Content));

        return new Response(StatusCode.s200, "Your article was received");
    }
}