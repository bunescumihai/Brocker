﻿// See https://aka.ms/new-console-template for more information

using System.Net.Sockets;
using System.Text;
using Brocker.Services;

int port = 8143;
string ip = "192.168.1.5";

CommandHandler commandHandler = CommandHandler.GetCommandHandler();

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
            
            commandHandler.HandleStringCommand(socket, stringCommand);
        }
        catch (Exception e)
        {
            Console.WriteLine("Disconnect");
            break;
        }

    }
}







