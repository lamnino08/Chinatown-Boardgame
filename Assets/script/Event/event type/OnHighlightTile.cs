public class OnHighlightTile
{
    public int tile;
    public bool isHighlight;
    public byte color;
    public OnHighlightTile(int tile, bool isHighlight, byte? color = 6)
    {
        this.tile = tile;
        this.isHighlight = isHighlight;
        this.color = (byte)color; 
    }
}