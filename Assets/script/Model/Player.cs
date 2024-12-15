using Colyseus.Schema;

public class Player : Schema
{
    [Type(0, "string")] public string sessionId;
    [Type(1, "string")] public string name;

    // Constructor to initialize the player
    public Player(string id, string playerName = "lamnino")
    {
        sessionId = id;
        name = string.IsNullOrEmpty(playerName) ? "lamnino" : playerName;
    }

    // Default constructor required by Colyseus
    public Player() { }
}
