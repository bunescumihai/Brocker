using Azure.Core;

namespace Brocker.Models;

public class Response
{
    public StatusCode StatusCode { get; set; }
    public string RequestId { get; set; }
    public object Content { get; set; }

    public Response(StatusCode statusCode, object content)
    {
        StatusCode = statusCode;
        Content = content;
    }
    
    public Response()
    { }
}