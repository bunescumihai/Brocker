namespace Brocker.Models;

public class Command <T> : CommandBase
{
    public T Content { get; set; }
    
    public Command(string name, T content)
    {
        Name = name;
        Content = content;
    }
}