
using UnityEngine;
[SerializeField]
public class ClientManager : MonoBehaviour
{
    [Header("User manager")]
    private int id;
    private int idserver = 2;
    public string Name = "Two";
    private Room room;
    public byte index = 0;
    public Member you;
    

    [Header("Room")]

    
    public static ClientManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }
    public void SetId(int id)
    {
        this.id = id;
    }
    public void SetIdServer(int idserver)
    {
        if (idserver == 0)
        this.idserver = idserver;
    }
    public void setRoom(Room room) => this.room = room;
    public Room GetRoom() => room;
    public int Id() => id;
    public int IdServer() => idserver;
    public void SetRoom(Room room)
    {
        this.room = room;
        you = room.members[room.members.Count-1];
        index  = (byte)room.members.Count;
    } 
    public void AddMember(Member new_mem)
    {
        if (room.members[room.members.Count - 1] != new_mem)
            room.members.Add(new_mem);
    }
    public void Outroom()
    {
        room = null;
        index = 0;
        you = null;
    }
    public void RemMember(int index)
    {
        room.members.RemoveAt(index-1);
    }
}
