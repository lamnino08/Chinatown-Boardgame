using System.Collections.Generic;

public class SpawnMarkEvent
{
    public IReadOnlyList<byte[]> tiles {get;}
    public SpawnMarkEvent(List<byte[]> data)
    {
        this.tiles = data;
    }
}
