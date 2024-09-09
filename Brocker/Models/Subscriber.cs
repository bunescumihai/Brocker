using System.Net.Sockets;

namespace Brocker.Models;

public class Subscriber
{
    public Socket Socket { get; }
    public List<Topic> Topics { get; }

    public Subscriber(Socket socket, List<Topic> topics)
    {
        Socket = socket;
        Topics = topics;
    }
}