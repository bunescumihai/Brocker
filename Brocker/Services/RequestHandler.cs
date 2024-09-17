using System.Net.Sockets;
using System.Text;
using Brocker.Exceptions;
using Brocker.Models;
using Brocker.Repositories;
using Brocker.Repositories.Implementations;
using Newtonsoft.Json;

namespace Brocker.Services;

public class RequestHandler
{

    private static RequestHandler _requestHandler = new RequestHandler();
    public static RequestHandler GetCommandHandler() => _requestHandler;
    private RequestHandler(){}

    private ConnectionsManager _connectionsManager = ConnectionsManager.GetConnectionsManager();
    
    public void HandleRequest(Socket socket, string stringCommand)
    {
        Console.WriteLine("Handle string command: " + stringCommand);
        CommandBase command = new CommandBase();
        
        Response response = new Response(StatusCode.s400, "No response");
        string requestId = "Null";
        
        try
        {
            Command<User>? userCommand;

            command = JsonConvert.DeserializeObject<CommandBase>(stringCommand);
            
            if (command is null)
                throw new JsonSerializationException();
            
            requestId = command.RequestId;
            
            if (command.Name.ToUpper().Equals(CommandType.TakeAnArticle.ToUpper()))
            {
                var artCommand = JsonConvert.DeserializeObject<AuthorizedCommand<Article>>(stringCommand);

                if(artCommand is null)
                    throw new JsonSerializationException();

                response = Core.HandleReceivedArticle(artCommand.Credentials , artCommand.Content);
            }

            else if (command.Name.ToUpper().Equals(CommandType.RegisterAsSender.ToUpper()))
            {
                userCommand = JsonConvert.DeserializeObject<Command<User>>(stringCommand);
                
                if(userCommand is null)
                    throw new JsonSerializationException();
                
                response = Core.RegisterAsReceiver(userCommand.Content);
            }   
            
            else if (command.Name.ToUpper().Equals(CommandType.RegisterAsReceiver.ToUpper()))
            {
                userCommand = JsonConvert.DeserializeObject<Command<User>>(stringCommand);
                
                if(userCommand is null)
                    throw new JsonSerializationException();
                
                response =  Core.RegisterAsSender(userCommand.Content);
            }

            else if (command.Name.ToUpper().Equals(CommandType.Subscribe.ToUpper()))
            {
                var com = JsonConvert.DeserializeObject<AuthorizedCommand<Topic>>(stringCommand);
                    
                if(com is null)
                    throw new JsonSerializationException();
                
                Console.WriteLine(JsonConvert.SerializeObject(com));
                
                response = Core.SubscribeToTopic(com.Credentials, com.Content);
            }
            
            else if (command.Name.ToUpper().Equals(CommandType.Unsubscribe.ToUpper()))
            {
                var tp = JsonConvert.DeserializeObject<AuthorizedCommand<Topic>>(stringCommand);
                
                if(tp is null)
                    throw new JsonSerializationException();
                
                response = Core.UnsubscribeFromTopic(tp.Credentials ,tp.Content);
            }
            
            else if (command.Name.ToUpper().Equals(CommandType.StartReceivingArticles.ToUpper()))
            {
                var cr = JsonConvert.DeserializeObject<Command<Credentials>>(stringCommand);
                
                if(cr is null)
                    throw new JsonSerializationException();

                IUserRepository ur = new UserRepository();

                var user = ur.GetUser(cr.Content.UserName, cr.Content.Password);
                
                if (user is null || user.UserRole == UserRole.Receiver)
                    throw new PermissionException();
                
                _connectionsManager.AddConnection(new Connection(){Socket = socket, User = user, RequestId = command.RequestId});
            }
            
            else if (command.Name.ToUpper().Equals(CommandType.GetTopics.ToUpper()))
            {
                response = Core.GetTopics();
            }
            
            else throw new BadCommandException();

        }
        catch (JsonReaderException e)
        {
            response = new Response(StatusCode.s400,  "Bad json format");
        }
        catch (JsonSerializationException e)
        {
            response = new Response(StatusCode.s400, "Bad object");
        }
        catch (SocketException e)
        {
            Console.WriteLine("Socket Exception");
        }
        catch (BadTopicException e)
        {
            response = new Response(StatusCode.s400, "Bad topic");
        }
        catch (UserExistsException e)
        {
            response = new Response(StatusCode.s400, e.Message);
        }
        catch (PermissionException e)
        {
            response = new Response(StatusCode.s400, e.Message);
        }
        catch (BadCommandException e)
        {
            response = new Response(StatusCode.s400, "Bad command");
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong in HandleStringCommand!");
        }

        response.RequestId = requestId;
        
        SendResponse(socket,  response);
    }
    
    private void SendResponse(Socket socket, Response response)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
        
        try
        {
            socket.Send(buffer);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}