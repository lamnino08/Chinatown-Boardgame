using System.Collections.Generic;
using Mirror;
using UnityEngine;

[System.Serializable]
public struct PlayerData
{
    public ulong id;
    public string name;
}

public class ListPlayerManager : NetworkBehaviour
{
    public static ListPlayerManager Instance { get; private set; }

    public SyncList<PlayerData> players = new SyncList<PlayerData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [Server]
    public void AddPlayer(ulong id, string name)
    {
        var playerData = new PlayerData { id = id, name = name };
        players.Add(playerData);
        RpcAddPlayer(id, name);
    }

    [Server]
    public void RemovePlayer(ulong id)
    {
        players.Remove(players.Find(player => player.id == id));
    }

    [ClientRpc]
    private void RpcAddPlayer(ulong id, string name)
    {
        LobbyPopupManager.instance.Toast("oke");
    }
    
}
