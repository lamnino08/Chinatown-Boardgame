using System.Collections.Generic;

/// <summary>
/// Spawn store card to player event
/// </summary> <summary>
/// 
/// </summary>
public class SpawnStoreCardEvent
{
    public IReadOnlyList<byte[]> storeCards { get; }

    public SpawnStoreCardEvent(List<byte[]> storeCards)
    {
        this.storeCards = storeCards;
    }
}

