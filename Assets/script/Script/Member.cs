using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Member 
{
    public int id;
    public int idserver;
    public string name;
    public List<byte> ground = new List<byte>();
    public List<byte> store_card = new List<byte>();
    public bool isready =false;
    public byte color = 0;
    public List<byte> ground_seperate = new List<byte>();
    public bool isExchange = false;
    public Member(int id, int idserver, string name)
    {
        this.id = id; this.idserver = idserver; this.name=name;
    }
    
    public override int GetHashCode()
    {
        return name.GetHashCode();
    }

}
