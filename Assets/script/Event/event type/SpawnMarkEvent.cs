using System.Collections.Generic;

/// <summary>
/// Spawn mark to board when all player chossen tile card
/// </summary> <summary>
/// 
/// </summary>
public class SpawnMarkEvent
{
    public IReadOnlyList<byte[]> tiles {get;}
    public SpawnMarkEvent(List<byte[]> data)
    {
        this.tiles = data;
    }
}
