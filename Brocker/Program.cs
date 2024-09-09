// See https://aka.ms/new-console-template for more information

using System.Net.Sockets;
using System.Text;
using Brocker.Models;
using Brocker.Services;
using Newtonsoft.Json;

int port = 8143;
string ip = "192.168.1.5";

CommandHandler commandHandler = new CommandHandler();

TcpConnectionListener tcpConnectionListener = new TcpConnectionListener(port, ip);

tcpConnectionListener.SocketEmitter += AttachToThread;


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

            Command<dynamic>? command = JsonConvert.DeserializeObject<Command<dynamic>>(stringCommand);
            
            if (command is not null)
            {
                if (command.Content.Data is Article)
                {
                    Command<Article> commandArticle = (Command<Article>)command;
                }
            {
                
            }
            else if (command.Content.Data is string)
            {
                Console.WriteLine("Topic Name: " + command.Data.Name);
            }
            }

        }
        catch (SocketException e)
        {
            
        }
        catch (Exception e)
        {
            
        }

    }
}








