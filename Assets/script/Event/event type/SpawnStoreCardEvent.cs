using System.Collections.Generic;

public class SpawnStoreCardEvent
{
    public IReadOnlyList<byte[]> storeCards { get; }

    public SpawnStoreCardEvent(List<byte[]> storeCards)
    {
        this.storeCards = storeCards;
    }
}

