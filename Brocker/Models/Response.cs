namespace Brocker.Models;

public class Response<T>
{
    public StatusCode StatusCode { get; }
    public T Content { get; }

    public Response(StatusCode statusCode, T content)
    {
        StatusCode = statusCode;
        Content = content;
    }
}