[System.Serializable]
public class Player
{
    public ulong id { get; private set; }
    public string name { get; private set; }

    public Player()
    {
        this.id = 0;
        this.name = "";
    }
    
    public Player(ulong id, string name)
    {
        this.id = id;
        this.name = name;
    }
}