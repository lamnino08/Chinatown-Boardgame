using Colyseus.Schema;

public class Room : Schema
{
    [Type(0, "map", typeof(MapSchema<Player>))]
    public MapSchema<Player> players = new MapSchema<Player>();

    public override string ToString()
    {
        return $"{players.Count}";
    }
}