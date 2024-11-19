using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    public uint id;
    public List<byte> ground  = new List<byte>();
    public List<byte> store  = new List<byte>();
    public List<byte> ground_seperate;
    public List<List<byte>> store_seperate;
    public List<byte> ground_chosse;
    /*
        Store information of member in room
    */
    public List<Member> members = new List<Member>();
    public bool ready;
    public byte year;
    public string state = "Chosse_Ground";
    public List<byte> colors = new List<byte>() {1,2,3,4,5};
    public Room(uint id_room, Member member)
    {
        this.id = id_room;
        members.Add(member);
    }
    public Room()
    {
    }
    public bool ContainMember(Member member)
    {
        foreach(Member mem in members)
        {
            Debug.Log(mem.name +"  "+ member.name);
            if (mem.name == member.name)
                return true;
        }
        return false;
    }
}
