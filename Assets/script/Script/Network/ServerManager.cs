using Mirror;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System;
using Unity.VisualScripting;
using Newtonsoft.Json;
public static class MatchExtension
{
    // Int to Guid
    public static Guid toGuid(this uint value)
    {
        using (var md5 = MD5.Create())
        {
            byte[] bytes = BitConverter.GetBytes(value);
            byte[] guidBytes = new byte[16];

            // Sao chép 4 byte đầu tiên từ bytes vào guidBytes
            Array.Copy(bytes, guidBytes, 4);

            // Đặt các byte còn lại trong guidBytes thành giá trị mặc định (0)
            for (int i = 4; i < 16; i++)
        {
            guidBytes[i] = 0;
        }

        return new Guid(guidBytes);
        }
    }
}
public class ServerManager : NetworkBehaviour
{
    public static ServerManager instance;
    public GameObject PrefabManager;
    /// <summary>
    /// list information connection of client 
    /// </summary>
    public static Dictionary<byte,NetworkConnection> players = new Dictionary<byte, NetworkConnection>();
    public static Room room = new Room();
    public static Dictionary<GameObject,MarkTranfer> transfer = new Dictionary<GameObject, MarkTranfer>();
    private void Start()
    {   
        if (isClient)
            Destroy(this);
        if (instance == null && isServer)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }
    /// <summary>
    /// Add new member who join game
    /// </summary>
    /// <param name="member"></param>
    /// <param name="player"></param>
    public void AddNewMember(Member member, Player player)
    {
        if (players.Count <4)
        {
            room.members.Add(member);
            players.Add((byte)(players.Count+1),player.Connection);
        }
    }
    /// <summary>
    /// Get color is available of room 
    /// </summary>
    /// <param name="id_room"></param>
    /// <returns></returns>
    [Server]
    public List<byte> Color_btn()
    {
        return room.colors;
    }
    /// <summary>
    /// valid One member ready to start game 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="color"></param>
    /// <param name="all_ready"></param>
    /// <returns></returns>
    [Server]
    public bool Ready(byte index,byte color, out bool all_ready)
    {
        //Check color
        if (!room.colors.Contains(color))
        {
            all_ready = false;
            return false;
        }
        // Set data
        room.members[index-1].color = color;
        room.members[index-1].isready = true;
        room.colors.Remove(color);
        // check all ready
        all_ready = room.members.Count > 1 ? isAllReady() : false;
        return true;
    }
    /// <summary>
    /// check all member is ready
    /// </summary>
    /// <returns></returns>
    bool isAllReady()
    {
        foreach(var player in room.members)
        {
            if (!player.isready) return false;
        }
        return true;
    }
    /// <summary>
    /// Valid Start game
    /// </summary>
    /// <returns></returns>
    [Server]
    public bool StartGame()
    {
        if (room.year == 0  && isAllReady())
        {
            room.year = 1;
            // reset ready state
            return true;
        }
        else
            return false;
    }
    /// <summary>
    /// Valid seperate for all member
    /// </summary>
    /// <returns></returns>
    // [Server]
    // public void SpawnPrefabManager()
    // {
    //     NetworkServer.Spawn(PrefabManager);
    // }
    [Server]
    public static List<List<byte>> SeperateGround()
    {
        if (room.year == 1)
            {
                for (byte i = 0; i <86; i++)
                    room.ground.Add(i);
                for (byte i = 0; i<12; i++)
                {
                    if (i< 3) room.store.Add(6);
                    else if (i<6) room.store.Add(7);
                    else if (i<9) room.store.Add(8);
                    else if (i<12) room.store.Add(9);
                }
            }
            byte numground = 0;
            byte numstore = 0;
            switch (room.members.Count)
            {
                case 3:
                    if (room.year == 1)
                    {
                        numground = 7;
                        numstore = 7;
                    }
                    else
                    {
                        numground = 6;
                        numstore = 4;
                    }
                    break;
                case 4:
                    if (room.year == 1)
                    {
                        numground = 6;
                        numstore = 6;
                    }
                    else
                    {
                        numground = 5;
                        numstore = 3;
                    }
                    break;
                case 5:
                    if (room.year < 4)
                    {
                        numground = 5;
                        if (room.year ==1)
                            numstore = 5;
                        else 
                            numstore = 3;
                    }
                    else
                    {
                        numground = 4;
                            numstore = 2;
                    }
                    break;
            }
            List<List<byte>> grounds = new List<List<byte>>();
            List<List<byte>> stores = new List<List<byte>>();
            foreach (var member in room.members )
            {
                //grounds
                member.isready = false;
                List<byte> ground = new List<byte>();
                for (byte i = 0; i< numground; i++)
                {
                    int grd = UnityEngine.Random.Range(1, room.ground.Count);
                    byte gr = room.ground[grd];
                    ground.Add(gr);
                    // Save ground seperate for each member
                    member.ground_seperate.Add(gr);
                    room.ground.Remove(gr); // remove ground seperate
                }
                grounds.Add(ground);
                // store
                List<byte> store = new List<byte>();
                for (byte i = 0; i< numstore; i++)
                {
                    byte str = 0;
                    do
                    {
                        str = (byte)UnityEngine.Random.Range(1,12);
                        if (room.store[str] > 0)
                        {
                            room.store[str]--;
                            break;
                        }
                    }
                    while (true);
                    store.Add(str);
                    // Save ground seperate for each member
                    member.store_card.Add(str);
                }
                stores.Add(store);
            }
            string storjson = JsonConvert.SerializeObject(stores);
            Debug.Log(storjson);
            room.store_seperate = stores;
            return grounds;
    }   
    // 
    [Server]
    public bool Commit_ground_chosse(int index,List<byte> Ground_return,out bool all_ready)
    {
        all_ready = true;
            /*
                Logic commit ground
            */
            Member member = room.members[index-1];
            member.isready = true;
            foreach(byte grrt in Ground_return)
            {
                if (member.ground_seperate.Contains(grrt))
                {
                    member.ground_seperate.Remove(grrt); 
                    room.ground.Add(grrt); // retrun plot to room
                }
            }
            // add seperate to member
            member.ground.AddRange(member.ground_seperate);

            //Check all member is ready
            all_ready = isAllReady();
            if (all_ready)
                room.state = "Negotiate";
            return true;
    }
    public static void tileNewOwn(byte tile)
    {
        foreach(MarkTranfer m in transfer.Values)
        {
            if (m.fromTile == tile)
            {
                m.isReplace = true;
                return;
            }
        }
    }
}   

