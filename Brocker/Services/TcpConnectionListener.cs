using System.Net;
using System.Net.Sockets;

namespace Brocker.Services;

public class TcpConnectionListener
{
    private int _port;
    private string _ip;
    private Socket listenerSocket;

    public delegate void EventHandler(Socket socket);

    public event EventHandler SocketEmitter;


    public TcpConnectionListener(int port, string ip)
    {
        _ip = ip;
        _port = port;
        BindSocket();
        StartListening();
    }

    private void BindSocket()
    {
        IPAddress ipAddress = IPAddress.Parse(_ip);
        IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, _port);

        listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listenerSocket.Bind(ipEndPoint);
    }

    private void StartListening()
    {
        listenerSocket.Listen(10);
        Thread listenerThread = new Thread(new ThreadStart(ConnectionListeningFunction));
        listenerThread.Start();
    }

    private void ConnectionListeningFunction()
    {
        while (true)
        {
            Socket socket = listenerSocket.Accept();
            SocketEmitter.Invoke(socket);
        }
    }
}