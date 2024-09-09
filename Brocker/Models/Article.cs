namespace Brocker.Models;

public class Article
{
    private Topic Topic { get; }
    private string Content { get; }

    public Article(Topic topic, string content)
    {
        Topic = topic;
        Content = content;
    }
}