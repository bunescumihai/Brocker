using Brocker.DbContexts;
using Brocker.Exceptions;
using Brocker.Models;

namespace Brocker.Repositories.Implementations;

public class UserRepository: IUserRepository
{
    private BrockerDbContext _brockerDbContext = BrockerDbContext.GetBrockerDbContext();
    
    public User RegisterLikeASender(string username, string password)
    {
        var user = new User(username, password, UserRole.Receiver);
        return SaveUser(user);
    }

    public User RegisterLikeAReceiver(string username, string password)
    {
        var user = new User(username, password, UserRole.Sender);
        return SaveUser(user);
    }

    private User SaveUser(User user)
    {
        if (UserExists(user))
            throw new UserExistsException(user);
        
        _brockerDbContext.Users.Add(user);

        try
        {   
            _brockerDbContext.SaveChanges();
        }
        catch (Exception e)
        {
            throw new SomethingWentWrongException("Something went wrong while saving your user");
        }

        return user;
    }

    private bool UserExists(User user)
    {
        return _brockerDbContext.Users.Any(u => u.UserName.Equals(user.UserName));
    }
}