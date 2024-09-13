using System.Net.Sockets;
using System.Text;
using Brocker.Exceptions;
using Brocker.Models;
using Newtonsoft.Json;

namespace Brocker.Services;

public class CommandHandler
{

    private static CommandHandler _commandHandler = new CommandHandler();
    
    public static CommandHandler GetCommandHandler() => _commandHandler;
    
    private CommandHandler(){}
    
    public void HandleStringCommand(Socket socket, string stringCommand)
    {
        Console.WriteLine("Handle string command: " + stringCommand);
        
        try
        {
            Command<String>? strCommand;
            Command<Article>? articleCommand;
            Command<Topic>? topicCommand;
            Command<User>? userCommand;

            var command = JsonConvert.DeserializeObject<CommandBase>(stringCommand);
            
            if (command is null)
                throw new JsonSerializationException();


            if (command.Name.ToUpper().Equals(CommandType.TakeAnArticle.ToUpper()))
            {
                var artCommand = JsonConvert.DeserializeObject<AuthorizedCommand<Article>>(stringCommand);

                if(artCommand is null)
                    throw new JsonSerializationException();

                Core.HandleReceivedArticle(socket, artCommand.Credentials , artCommand.Content);
            }
            
            
            else if (command.Name.ToUpper().Equals(CommandType.RegisterAsSender.ToUpper()))
            {
                userCommand = JsonConvert.DeserializeObject<Command<User>>(stringCommand);
                
                if(userCommand is null)
                    throw new JsonSerializationException();
                
                Core.RegisterAsReceiver(socket, userCommand.Content);
            }   
            
            else if (command.Name.ToUpper().Equals(CommandType.RegisterAsReceiver.ToUpper()))
            {
                userCommand = JsonConvert.DeserializeObject<Command<User>>(stringCommand);
                
                if(userCommand is null)
                    throw new JsonSerializationException();
                
                Core.RegisterAsSender(socket, userCommand.Content);
            }


            else if (command.Name.ToUpper().Equals(CommandType.Subscribe.ToUpper()))
            {
                topicCommand = JsonConvert.DeserializeObject<Command<Topic>>(stringCommand);
                    
                if(topicCommand is null)
                    throw new JsonSerializationException();

                Core.SubscribeToTopic(socket, topicCommand.Content);
            }
            
            
            else if (command.Name.ToUpper().Equals(CommandType.Unsubscribe.ToUpper()))
            {
                topicCommand = JsonConvert.DeserializeObject<Command<Topic>>(stringCommand);
                
                if(topicCommand is null)
                    throw new JsonSerializationException();
                
                Core.UnsubscribeFromTopic(socket, topicCommand.Content);
            }
            
            
            else if (command.Name.ToUpper().Equals(CommandType.GetTopics.ToUpper()))
            {
                Core.SendTopics(socket);
            }
            else throw new BadCommandException();

        }
        catch (JsonReaderException e)
        {
            Core.SendSimpleResponse(socket, new Response<string>(StatusCode.s400, "Bad json format"));
        }
        catch (JsonSerializationException e)
        {
            Core.SendSimpleResponse(socket, new Response<string>(StatusCode.s400, "Bad object"));
        }
        catch (SocketException e)
        {
            Console.WriteLine("Socket Exception");
        }
        catch (BadTopicException e)
        {
            Core.SendSimpleResponse(socket, new Response<string>(StatusCode.s400, "Bad topic"));
        }
        catch (UserExistsException e)
        {
            Core.SendSimpleResponse(socket, new Response<string>(StatusCode.s400, e.Message));
        }
        catch (BadCommandException e)
        {
            Console.WriteLine("Bad Command");
            
            var sb = new StringBuilder();

            sb.Append("Bad command. Available commands:\n");
            sb.Append("1. Subscribe.\n");
            sb.Append("2. Unsubscribe.\n");
            sb.Append("3. GetTopics.\n");
            sb.Append("4. TakeAnArticle.\n");

            Core.SendSimpleResponse(socket, new Response<string>(StatusCode.s400, sb.ToString()));
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong in HandleStringCommand!");
        }
    }
}