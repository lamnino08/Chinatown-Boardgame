using System;

[Serializable]
public class CardDealResult
{
    public int tile;
    public bool isChosse;
    public CardDealResult()
    {
        tile = 0;
        isChosse = false;
    }
    public CardDealResult(int tile, bool isChosse)
    {
        this.tile = tile;
        this.isChosse = isChosse;
    }
}