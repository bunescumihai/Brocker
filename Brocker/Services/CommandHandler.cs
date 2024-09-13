using System.Net.Sockets;
using System.Text;
using Brocker.Exceptions;
using Brocker.Models;
using Newtonsoft.Json;

namespace Brocker.Services;

public class CommandHandler
{
    public void HandleStringCommand(Socket socket, string stringCommand)
    {
        Console.WriteLine("Handle string command: " + stringCommand);
        try
        {
            Command<String>? strCommand;
            Command<Article>? articleCommand;
            Command<Topic>? topicCommand;

            var command = JsonConvert.DeserializeObject<CommandBase>(stringCommand);
            
            if (command is null)
                throw new JsonSerializationException();


            if (command.Name.ToUpper().Equals(CommandType.TakeAnArticle.ToUpper()))
            {
                articleCommand = JsonConvert.DeserializeObject<Command<Article>>(stringCommand);
                
                if(articleCommand is null)
                    throw new JsonSerializationException();
                
                Core.HandleReceivedArticle(socket, articleCommand.Content);
            }
            else if (command.Name.ToUpper() == CommandType.Subscribe.ToUpper())
            {
                topicCommand = JsonConvert.DeserializeObject<Command<Topic>>(stringCommand);
                
                if(topicCommand is null)
                    throw new JsonSerializationException();

                Core.SubscribeToTopic(socket, topicCommand.Content);
            }
            else if (command.Name.ToUpper() == CommandType.Unsubscribe.ToUpper())
            {
                topicCommand = JsonConvert.DeserializeObject<Command<Topic>>(stringCommand);
                
                if(topicCommand is null)
                    throw new JsonSerializationException();
                
                Core.UnsubscribeFromTopic(socket, topicCommand.Content);
            }
            else if (command.Name.ToUpper() == CommandType.GetTopics.ToUpper())
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