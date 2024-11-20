[System.Serializable]
public class PlayerData
{
    public ulong id;
    public string name;

    public PlayerData()
    {
        this.id = 0;
        this.name = "";
    }
    
    public PlayerData(ulong id, string name)
    {
        this.id = id;
        this.name = name;
    }
}