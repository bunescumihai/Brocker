namespace Brocker.Models;

public enum StatusCode
{
    s404 = 404, // Topic not found
    s401 = 401, // Unauthorized
    s400 = 400, // Bad Command
    s200 = 200, // Ok
}