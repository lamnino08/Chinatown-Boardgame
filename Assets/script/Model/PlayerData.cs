using System.Drawing;

[System.Serializable]
public class PlayerData
{
    public ulong id { get; private set; }
    public string name { get; private set; }
    public bool isReady { get; private set; }
    public byte color { get; private set; } 

    public PlayerData()
    {
        this.id = 0;
        this.name = "";
        this.color = 0;
        this.isReady = false;
    }
    
    public PlayerData(ulong id, string name)
    {
        this.id = id;
        this.name = name;
        this.isReady = false;
        this.color = 0;
    }

    public void SetReady(bool ready)
    {
        this.isReady = ready;
    }

    public void SetColor(byte color)
    {
        this.color = color;
    }
}