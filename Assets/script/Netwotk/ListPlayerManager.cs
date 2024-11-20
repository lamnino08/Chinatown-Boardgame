using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ListPlayerManager : NetworkBehaviour
{
    public static ListPlayerManager instance { get; private set; }

    private List<PlayerData> _players = new List<PlayerData>();
    public List<PlayerData> players => _players;
    // list connection 
    private Dictionary<ulong, NetworkConnection> playerConnections = new Dictionary<ulong, NetworkConnection>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    [Server]
    public void AddPlayer(PlayerData newPlayer, NetworkConnection conn)
    {
        _players.Add(newPlayer);

         if (!playerConnections.ContainsKey(newPlayer.id))
        {
            playerConnections[newPlayer.id] = conn;
        }
    }

    [Server]
    public void RemovePlayer(ulong id)
    {
        _players.Remove(_players.Find(player => player.id == id));
    }
}
