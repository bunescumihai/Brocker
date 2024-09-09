namespace Brocker.Models;

public class Command <T>
{
    public string Name { get; set; }
    public T Content { get; set; }
    
    public Command(string name, T content)
    {
        Name = name;
        Content = content;
    }
}