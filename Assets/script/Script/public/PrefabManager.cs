using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PrefabManager : NetworkBehaviour
{
    public static PrefabManager instance = null;
    [Header("Player prebfab in main game")]
    [SerializeField] public GameObject MemberSlot;
    [SerializeField] public List<Transform> posSpawnMembers;
    [Header("item")]
    [SerializeField] public GameObject tile;
    [SerializeField] public List<GameObject> markprebs;
    [SerializeField] public List<GameObject> storeCard;
    [SerializeField] public List<GameObject> Bowl_marks;

    // override public  void OnStartClient()
    // {
    //     Destroy(this); return;
    // }
     public void Start()
    {
        if (instance == null) 
        {
            instance = this;
            // Spawn member and Tile in Game
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }
    /// <summary>
    /// Spawn Game player gaeme object
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="angle"></param>
    [Server]
    public void SpawnMemberInGame()
    {
        // Spawn tile
        Debug.Log("StartGame");
            Instantiate(ServerManager.instance.PrefabManager);
            // GameObject tile = Instantiate(PrefabManager.instance.tile);
            Game.tile = Instantiate(PrefabManager.instance.tile);
            NetworkServer.Spawn(Game.tile);
        // Spawn Member slot
            foreach (var index in ServerManager.players.Keys)
            {
                GameObject member = Instantiate(PrefabManager.instance.MemberSlot,PrefabManager.instance.posSpawnMembers[index-1].position, Quaternion.Euler(0,GameManager.Angle_Spawn(index),0));
                member.GetComponent<Game>().own = index;
                NetworkServer.Spawn(member, ServerManager.players[(byte)(index)]);
                member.GetComponent<Game>().Spawn_BowlMark(ServerManager.room.members[index-1].color, ServerManager.players[(byte)(index)]);
                // member.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            }
            // Seperatecard
            
        
    }
    
}
