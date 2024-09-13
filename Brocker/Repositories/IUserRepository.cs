namespace Brocker.Repositories;

public interface IUserRepository
{
    User RegisterLikeASender(string username, string password);
    User RegisterLikeAReceiver(string username, string password);
}