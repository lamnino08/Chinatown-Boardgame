using System.Collections.Generic;

public class Room 
{
    private List<byte> _colors = new List<byte>{0,1,2,3,4};
    public List<byte> colors => _colors;
    
    public Room()
    {

    }

    public void RemoveColor(byte color)
    {
        _colors.Remove(color);
    }
}
