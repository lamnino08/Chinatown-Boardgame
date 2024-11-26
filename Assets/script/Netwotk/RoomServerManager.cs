using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RoomServerManager : NetworkBehaviour
{
    public static RoomServerManager instance { get; private set; }

    private List<PlayerData> _players = new List<PlayerData>();
    public Room room { get; private set; }
    public List<PlayerData> players => _players;
    // list connection 
    private List<NetworkConnection> _playerConnections = new List<NetworkConnection>();
    public List<NetworkConnection> playerConnections => _playerConnections;
    // private Dictionary<int, NetworkConnection> playerConnections = new Dictionary<int, NetworkConnection>();

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
            return;
        }

        room = new Room();
    }

    [Server]
    public void AddPlayer(PlayerData newPlayer, NetworkConnection conn)
    {
        _players.Add(newPlayer);
        _playerConnections.Add(conn);
    }

    [Server]
    public void RemovePlayer(ulong id)
    {
        _players.Remove(_players.Find(player => player.id == id));
    }

    [Server] 
    public List<byte> GetAvailableColors()
    {
        return room.colors;
    }

    [Server]
    public void NewYear()
    {
        List<byte[]> tiles = room.NewYear(_players.Count);
        for (int i = 0; i < _players.Count; i++)
        {
            PlayerManager._host.DistributeTiles( _playerConnections[i], tiles[i]);
        }
    }
}
