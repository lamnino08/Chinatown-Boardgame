using Colyseus.Schema;

public class Lobby : Schema
{
    [Type(0, "map", typeof(MapSchema<string>), "string")]
	public MapSchema<string> colors = new MapSchema<string>();

    [Type(1, "map", typeof(MapSchema<PlayerLobby>))]
    public MapSchema<PlayerLobby> players = new MapSchema<PlayerLobby>();

    public string GetNamePlayerBySessionID(string sessionID)
    {
        PlayerLobby value;
        players.TryGetValue(sessionID, out value);
        return value.name;
    }
}
