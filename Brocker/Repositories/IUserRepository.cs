namespace Brocker.Repositories;

public interface IUserRepository
{
    User RegisterLikeASender(string username, string password);
    User RegisterLikeAReceiver(string username, string password);
    List<Article> GetSenderArticles(User user);

    User? GetUser(string username, string password);
}