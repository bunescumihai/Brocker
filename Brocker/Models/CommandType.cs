namespace Brocker.Models;

public static class CommandType
{
    public static string Subscribe = "Subscribe";
    public static string Unsubscribe = "Unsubscribe";
    public static string GetTopics = "GetTopics";
    public static string TakeAnArticle = "TakeAnArticle";
    public static string RegisterAsSender = "RegisterAsSender";
    public static string RegisterAsReceiver = "RegisterAsReceiver";
}