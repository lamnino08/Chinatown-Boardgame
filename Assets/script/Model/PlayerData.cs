using System.Drawing;

[System.Serializable]
public class PlayerData
{
    public ulong id;
    public string name;
    public bool isReady;
    public byte color;

    public PlayerData()
    {
        this.id = 0;
        this.name = "";
        this.color = 6;
        this.isReady = false;
    }
    
    public PlayerData(ulong id, string name)
    {
        this.id = id;
        this.name = name;
        this.isReady = false;
        this.color = 6;
    }

    public void SetReady(bool ready)
    {
        this.isReady = ready;
    }

    public void SetColor(byte color)
    {
        this.color = color;
    }

    public override string ToString()
    {
        return  $"PlayerData [ID: {id}, Name: {name}, Ready: {isReady}, Color: {color}]";
    }
}