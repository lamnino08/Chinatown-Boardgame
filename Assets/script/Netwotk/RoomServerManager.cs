using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class RoomServerManager : NetworkBehaviour
{
    public static RoomServerManager instance { get; private set; }

    public Room room { get; private set; }
    private List<PlayerData> _players = new List<PlayerData>();
    public List<PlayerData> players => _players;
    // list connection 
    private List<NetworkConnection> _playerConnections = new List<NetworkConnection>();
    public List<NetworkConnection> playerConnections => _playerConnections;

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
    public void PlayerReady(int playerIndex, byte color)
    {
        room.RemoveColor(color);
        _players[playerIndex].SetReady(true);
        _players[playerIndex].SetColor(color);
    }

    [Server]
    public bool IsAllReady()
    {
        return _players.All(player => player.isReady);
    }

    [Server]
    public void NewYear()
    {
        List<byte[]> tiles = room.NewYear(_players.Count);
        for (int i = 0; i < _players.Count; i++)
        {
            players[i].SetReady(false);
            PlayerManager._host.DistributeTiles(_playerConnections[i], tiles[i]);
        }
    }

    [Server]
    public static List<byte[]> DistributeStoreCard()
    {
        return instance.room.DistributeStoreCard(instance.players.Count);
    }

    [Server]
    public void ReceiveResultChoseTileCard(List<TileCardReturnServer> tileReturn, int indexPlayer)
    {
        room.ReceiveResultChoseTileCard(tileReturn);
        _players[indexPlayer].SetReady(true);

        if (!IsAllReady()) return;

        // Spawn store card and mark when all player chosse tile card
        GameServerManager.instance.SpawnStoreCard();
        GameServerManager.instance.SpawnMark();
    }
}
