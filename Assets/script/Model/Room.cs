using Colyseus.Schema;

public class Room : Schema
{
    [Type(0, "string")]
	public string gamePhare = default(string);

	[Type(1, "map", typeof(MapSchema<Player>))]
	public MapSchema<Player> players = new MapSchema<Player>();

    public override string ToString()
    {
        return $"{players.Count}";
    }

    public GamePhase GetGamePhase()
    {
        if (System.Enum.TryParse<GamePhase>(gamePhare, true, out var phase))
        {
            return phase;
        }
        else
        {
            UnityEngine.Debug.LogWarning($"Invalid GamePhase: {gamePhare}");
            return GamePhase.WAITING; // Default nếu không parse được
        }
    }
}
