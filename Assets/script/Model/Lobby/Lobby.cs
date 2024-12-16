using Colyseus.Schema;

public class Lobby : Schema
{
    [Type(0, "array", typeof(ArraySchema<bool>), "boolean")]
	public ArraySchema<bool> colors = new ArraySchema<bool>();

	[Type(1, "map", typeof(MapSchema<PlayerLobby>))]
	public MapSchema<PlayerLobby> players = new MapSchema<PlayerLobby>();

    public string GetNamePlayerBySessionID(string sessionID)
    {
        PlayerLobby value;
        players.TryGetValue(sessionID, out value);
        return value.name;
    }

    public bool IsAllReady()
    {
         foreach (PlayerLobby player in players.Values)
        {
            if (!player.isReady)
            {
                return false; // Nếu có bất kỳ player nào không sẵn sàng, trả về false
            }
        }
        return true;
    }
}
