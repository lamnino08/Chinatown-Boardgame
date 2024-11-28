[System.Serializable]
public class PlayerData
{
    public ulong id;
    public string name;
    public bool isReady;

    public PlayerData()
    {
        this.id = 0;
        this.name = "";
        this.isReady = false;
    }
    
    public PlayerData(ulong id, string name)
    {
        this.id = id;
        this.name = name;
        this.isReady = false;
    }
}