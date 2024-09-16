using System.Net.Sockets;
using Brocker.Exceptions;
using Brocker.Models;
using Brocker.Repositories;
using Brocker.Repositories.Implementations;
using Newtonsoft.Json;

namespace Brocker.Services;

public class CommandHandler
{

    private static CommandHandler _commandHandler = new CommandHandler();
    public static CommandHandler GetCommandHandler() => _commandHandler;
    private CommandHandler(){}

    private ConnectionsManager _connectionsManager = ConnectionsManager.GetConnectionsManager();
    
    public void HandleStringCommand(Socket socket, string stringCommand)
    {
        Console.WriteLine("Handle string command: " + stringCommand);
        CommandBase command = new CommandBase();
        try
        {
            Command<User>? userCommand;

            command = JsonConvert.DeserializeObject<CommandBase>(stringCommand);
            
            if (command is null)
                throw new JsonSerializationException();
            
            if (command.Name.ToUpper().Equals(CommandType.TakeAnArticle.ToUpper()))
            {
                var artCommand = JsonConvert.DeserializeObject<AuthorizedCommand<Article>>(stringCommand);

                if(artCommand is null)
                    throw new JsonSerializationException();

                Core.HandleReceivedArticle(socket, command.RequestId, artCommand.Credentials , artCommand.Content);
            }

            else if (command.Name.ToUpper().Equals(CommandType.RegisterAsSender.ToUpper()))
            {
                userCommand = JsonConvert.DeserializeObject<Command<User>>(stringCommand);
                
                if(userCommand is null)
                    throw new JsonSerializationException();
                
                Core.RegisterAsReceiver(socket, command.RequestId, userCommand.Content);
            }   
            
            else if (command.Name.ToUpper().Equals(CommandType.RegisterAsReceiver.ToUpper()))
            {
                userCommand = JsonConvert.DeserializeObject<Command<User>>(stringCommand);
                
                if(userCommand is null)
                    throw new JsonSerializationException();
                
                Core.RegisterAsSender(socket, command.RequestId, userCommand.Content);
            }

            else if (command.Name.ToUpper().Equals(CommandType.Subscribe.ToUpper()))
            {
                var com = JsonConvert.DeserializeObject<AuthorizedCommand<Topic>>(stringCommand);
                    
                if(com is null)
                    throw new JsonSerializationException();
                
                Console.WriteLine(JsonConvert.SerializeObject(com));
                
                Core.SubscribeToTopic(socket, command.RequestId, com.Credentials, com.Content);
            }
            
            else if (command.Name.ToUpper().Equals(CommandType.Unsubscribe.ToUpper()))
            {
                var tp = JsonConvert.DeserializeObject<AuthorizedCommand<Topic>>(stringCommand);
                
                if(tp is null)
                    throw new JsonSerializationException();
                
                Core.UnsubscribeFromTopic(socket, command.RequestId, tp.Credentials ,tp.Content);
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
                Core.SendTopics(socket, command.RequestId);
            }
            
            else throw new BadCommandException();

        }
        catch (JsonReaderException e)
        {
            Core.SendSimpleResponse(socket, new Response<string>(StatusCode.s400, command.RequestId,  "Bad json format"));
        }
        catch (JsonSerializationException e)
        {
            Core.SendSimpleResponse(socket, new Response<string>(StatusCode.s400, command.RequestId,"Bad object"));
        }
        catch (SocketException e)
        {
            Console.WriteLine("Socket Exception");
        }
        catch (BadTopicException e)
        {
            Core.SendSimpleResponse(socket, new Response<string>(StatusCode.s400,command.RequestId, "Bad topic"));
        }
        catch (UserExistsException e)
        {
            Core.SendSimpleResponse(socket, new Response<string>(StatusCode.s400,command.RequestId, e.Message));
        }
        catch (PermissionException e)
        {
            Core.SendSimpleResponse(socket, new Response<string>(StatusCode.s400,command.RequestId, e.Message));
        }
        catch (BadCommandException e)
        {
            Core.SendSimpleResponse(socket, new Response<string>(StatusCode.s400,command.RequestId, "Bad command"));
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong in HandleStringCommand!");
        }
    }
}