using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using JetBrains.Annotations;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager _host;
    public static PlayerManager host => _host;

    [SyncVar]
    public string playerName;

    [SyncVar]
    public int index = 0;

    [SyncVar]
    private ulong playerId;

    [SyncVar]
    private byte color = 6;

    private List<byte> tiles = new List<byte>();
    public bool isHost => isServer && isClient;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public override void OnStartLocalPlayer()
    {
        CmdSetPlayerData(PlayerPrefs.GetString("PlayerName"));
        if (isHost)
        {
            if (LobbyUIManager.instance != null)
                LobbyUIManager.instance.SetupUI(isHost);
        }
        GameMaster.instance.SetLocalPlayer(this);
    }   

    public override void OnStartServer()
    {
        if (isHost) _host = this;
    }

    [Server]
    public void CmdSpawnPlayerSlot()
    {
        GameServerManager.instance.SpawnPlayerSlot();
    }

    // Start on local
    [Command]
    private void CmdSetPlayerData(string name)
    {
        ulong randomID = (ulong)System.DateTime.Now.Ticks;
        playerId = randomID;
        playerName = name;

        PlayerData newPlayer = new PlayerData(randomID, name );

        GetDataInLobby(connectionToClient);
        RoomServerManager.instance.AddPlayer(newPlayer, connectionToClient);
        index = RoomServerManager.instance.players.Count - 1;
        RpcNewPlayerUI(newPlayer);
    }

    // Ready 
    [Command]
    public void CmdReady()
    {
        List<byte> availableColors = RoomServerManager.instance.GetAvailableColors();
        bool isSuccess = availableColors.Contains(color);
        if (isSuccess)
        {
            RoomServerManager.instance.room.RemoveColor(color);
            RoomServerManager.instance.PlayerReady(index, color);
            RpcPlayerReady(playerName, Util.TransferColor(color));
        }
        TargetReadyResult(connectionToClient, isSuccess);
    }

    // Get available color before open Chose color popup
    [Command]
    public void OpenColorPupup()
    {
        List<byte> availableColors = RoomServerManager.instance.GetAvailableColors();
        RpcColorToOpenChosseColor(connectionToClient, availableColors);
    }

    [Command]
    public void SetColor(byte color)
    {
        this.color = color;
    }

    [Command]
    public void StartGame()
    {
        if (isHost)
        {
            NetworkManager.singleton.ServerChangeScene("GameScene");
            var playerDataArray = RoomServerManager.instance.players.ToArray();
        }
    }

    [Command]
    public void NewYear()
    {
        RoomServerManager.instance.NewYear();
    }

    [Command]
    public void ConfirmTileCard(List<TileCardReturnServer> result)
    {
        byte[] tileChosen = new byte[result.Count - 2];
        for (int i = result.Count - 1; i >= 0; i--) 
        {
            int indexTileChose = 0;
            if (result[i].isChosse)
            {
                tileChosen[indexTileChose] = result[i].tile;
                indexTileChose++;
                tiles.Add(result[i].tile);
                result.RemoveAt(i); // Xóa phần tử khỏi danh sách
            }
        }

        GameServerManager.instance.tileSpawnMarkSave.Add(tileChosen);
        RoomServerManager.instance.ReceiveResultChoseTileCard(result, index);
    }


    // Server handle client left
    [Server]
    public void CmdOnStopClient()
    {
        RpcRemoveUserUI(playerName);
    }

    // Get players already in room when join
    [Server]
    public void GetDataInLobby(NetworkConnectionToClient  connectionToClient)
    {
        var playerDataArray = RoomServerManager.instance.players.ToArray();
        RpcPlayerListUI(connectionToClient, playerDataArray);
    }

    // Handle Ui when a player left
    [ClientRpc]
    private void RpcRemoveUserUI(string name)
    {
        LobbyPopupManager.instance.Toast($"Player {name} has left the game.");
        LobbyUIManager.instance.RemovePlayerUI(name);
    }

    // UI when player join room
    [ClientRpc]
    private void RpcNewPlayerUI(PlayerData newPlayer)
    {
        LobbyUIManager.instance.AddNewPlayerUI(newPlayer);
    }

    [ClientRpc]
    private void RpcPlayerReady(string name, Color color)
    {
        LobbyUIManager.instance.PlayerReady(name, color);
    }

    // Render list User in room
    [TargetRpc]
    private void RpcPlayerListUI(NetworkConnectionToClient connectionToClient, PlayerData[] players)
    {
        if (LobbyUIManager.instance != null)
            LobbyUIManager.instance.SetSlotPlayerUI(players);
    }

    [TargetRpc]
    private void RpcColorToOpenChosseColor(NetworkConnectionToClient connectionToClient, List<byte> availableColors)
    {
        LobbyPopupManager.instance.ShowChoseColorPopup(availableColors);
    }

    // ready result
    [TargetRpc]
    private void TargetReadyResult(NetworkConnectionToClient target, bool isSuccess)
    {
        LobbyUIManager.instance.OnReadyResult(isSuccess);
    }

    [TargetRpc]
    public void DistributeTiles(NetworkConnection conn, byte[] tiles)
    {
        GameMaster.instance.deskCard.DiscardToPlayer(tiles);
        GameUIManager.instance.ReceiveCardDiscard();
    }

    
}
