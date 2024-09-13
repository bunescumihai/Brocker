namespace Brocker.Exceptions;

public class UserExistsException: Exception
{
    public UserExistsException(User user) : base($"User with username '{user.UserName}' already exists") { }
}