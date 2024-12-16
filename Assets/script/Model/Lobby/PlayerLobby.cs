using Colyseus.Schema;

public class PlayerLobby : Schema
{
    [Type(0, "string")]
	public string name = default(string);

	[Type(1, "boolean")]
	public bool isReady = default(bool);

	[Type(2, "int32")]
	public int color = default(int);        
}
