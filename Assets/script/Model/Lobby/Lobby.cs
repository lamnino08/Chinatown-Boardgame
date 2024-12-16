using Colyseus.Schema;
using UnityEngine;

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
                Debug.Log($"{player.name}");
                return false; 
            }
        }
        return true;
    }
}
