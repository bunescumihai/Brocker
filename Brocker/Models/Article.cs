namespace Brocker.Models;

public class Article
{
    public Topic Topic { get; }
    public string Content { get; }

    public Article(Topic topic, string content)
    {
        Topic = topic;
        Content = content;
    }
}