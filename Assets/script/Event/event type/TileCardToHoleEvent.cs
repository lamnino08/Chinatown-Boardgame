using UnityEngine;

/// <summary>
/// Event card chossen fly to owner hole card
/// </summary>
public class TileCardToHoleEvent
{
    public Transform deskHoleTranform;
    public TileCardToHoleEvent(Transform deskTransform)
    {
        this.deskHoleTranform = deskTransform;
    }
}
