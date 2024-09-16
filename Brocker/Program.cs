// See https://aka.ms/new-console-template for more information

using System.Net.Sockets;
using System.Text;
using Brocker.Models;
using Brocker.Repositories;
using Brocker.Repositories.Implementations;
using Brocker.Services;
using Newtonsoft.Json;

int port = 8143;
string ip = "192.168.1.5";

CommandHandler commandHandler = CommandHandler.GetCommandHandler();

TcpConnectionListener tcpConnectionListener = new TcpConnectionListener(port, ip);

tcpConnectionListener.SocketEmitter += AttachToThread;

Thread articleSenderThread = new Thread(new ThreadStart(SendArticles));
articleSenderThread.Start();

void AttachToThread(Socket socket)
{
    Thread thread = new Thread(new ThreadStart(() => { ReceiveMessage(socket); }));
    thread.Start();
}

void ReceiveMessage(Socket socket)
{
    StringBuilder sb = new StringBuilder();

    while (true)
    {
        try
        {
            do
            {
                byte[] buffer = new byte[1024];
                int bytesReceived = socket.Receive(buffer);

                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesReceived));

            } while (socket.Available > 0);

            string stringCommand = sb.ToString();
            sb.Clear();
            
            commandHandler.HandleStringCommand(socket, stringCommand);
        }
        catch (Exception e)
        {
            Console.WriteLine("Disconnect");
            break;
        }

    }
}

void SendArticles()
{
    IArticleRepository articleRepository = new ArticleRepository();
    
    ConnectionsManager connectionsManager = ConnectionsManager.GetConnectionsManager();
    
    foreach (var connection in connectionsManager.Connections)
    {
        var user = connection.User;
        if (SocketConnected(connection.Socket))
        {
            var articles = articleRepository.GetUnsentArticles(user);
            
            if(articles.Count < 1)
                continue;
            
            var response = new Response<List<Article>>(StatusCode.s200, connection.RequestId, articles);
            
            var st = JsonConvert.SerializeObject(response);
            
            byte[] bytes = Encoding.UTF8.GetBytes(st);
            try
            {
                connection.Socket.Send(bytes);
                articleRepository.CancelSending(user, articles);
            }
            catch (Exception e)
            {
                connectionsManager.AddConnectionToRemove(connection);
            }
        }
    }
    
    connectionsManager.RemoveConnections();
    
    Thread.Sleep(1000);
}



bool SocketConnected(Socket s)
{
    bool part1 = s.Poll(1000, SelectMode.SelectRead);
    bool part2 = (s.Available == 0);
    
    if (part1 && part2)
        return false;
    else
        return true;
}





