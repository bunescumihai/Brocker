using System.Net.Sockets;

namespace Brocker.Services;

public class Connection
{
    public Socket Socket { get; set; }
    public User User { get; set; }
}