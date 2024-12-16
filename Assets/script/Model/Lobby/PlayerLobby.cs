using Colyseus.Schema;

public class PlayerLobby : Schema
{
    [Type(0, "string")]
	public string name;

	[Type(1, "boolean")]
	public bool isReady;

	[Type(2, "int32")]
	public int color;      
}
