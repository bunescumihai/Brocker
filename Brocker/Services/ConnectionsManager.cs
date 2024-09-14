namespace Brocker.Services;

public class ConnectionsManager
{
    private static ConnectionsManager _connectionsManager = new ConnectionsManager();

    public List<Connection> Connections { get; } = new List<Connection>();
    public List<Connection> ConnectionsToRemove { get; } = new List<Connection>();
    
    public static ConnectionsManager GetConnectionsManager() => _connectionsManager;
    private ConnectionsManager(){}

    public void AddConnection(Connection connection) => Connections.Add(connection);
    
    public void AddConnectionToRemove(Connection connection) => ConnectionsToRemove.Add(connection);
    
    public void RemoveConnections()
    {
        foreach (var conn in ConnectionsToRemove)
        {
            Connections.Remove(conn);
        }
    }
    
}