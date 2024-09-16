using Azure.Core;

namespace Brocker.Models;

public class Response<T>
{
    public StatusCode StatusCode { get; }
    public string RequestId { get; }
    public T Content { get; }

    public Response(StatusCode statusCode, string requestId, T content)
    {
        StatusCode = statusCode;
        Content = content;
        RequestId = requestId;
    }
}