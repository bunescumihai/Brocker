namespace Brocker.Services;

public class ConnectionsManager
{
    private static ConnectionsManager _connectionsManager = new ConnectionsManager();

    private List<Connection> Connections { get; } = new List<Connection>();
    
    public static ConnectionsManager GetConnectionsManager() => _connectionsManager;
    private ConnectionsManager(){}

    public void AddConnection(Connection connection) => Connections.Add(connection);
}