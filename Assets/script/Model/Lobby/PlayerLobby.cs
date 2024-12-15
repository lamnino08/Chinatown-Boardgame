using Colyseus.Schema;

public class PlayerLobby : Schema
{
    [Type(0, "string")] public string name;       // string
    [Type(1, "boolean")] public bool isReady;     // boolean
    [Type(2, "string")] public string color;        
}
