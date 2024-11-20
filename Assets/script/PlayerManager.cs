using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using JetBrains.Annotations;

public class PlayerManager : NetworkBehaviour
{
    private static PlayerManager _host;
    public static PlayerManager host => _host;

    [SyncVar]
    private string playerName;

    [SyncVar]
    public byte index;

    [SyncVar]
    private ulong playerId;

    [SyncVar]
    private byte color = 6;

    public bool isHost => isServer && isClient;

    public override void OnStartLocalPlayer()
    {
        CmdSetPlayerData(PlayerPrefs.GetString("PlayerName"));
        if (isHost)
        {
            LobbyUIManager.instance.SetupUI(isHost);
        }
        GameMaster.instance.SetLocalPlayer(this);
    }

    public override void OnStartServer()
    {
        if (isHost) _host = this;
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
        ListPlayerManager.instance.AddPlayer(newPlayer, connectionToClient);
        RpcNewPlayerUI(newPlayer);
    }

    // Ready 
    [Command]
    public void CmdReady()
    {
        List<byte> availableColors = ListPlayerManager.instance.GetAvailableColors();
        bool isSuccess = availableColors.Contains(color);
        if (isSuccess)
        {
            ListPlayerManager.instance.room.RemoveColor(color);
            RpcPlayerReady(playerName, Util.TransferColor(color));
        }
        TargetReadyResult(connectionToClient, isSuccess);
    }

    // Get available color before open Chose color popup
    [Command]
    public void OpenColorPupup()
    {
        List<byte> availableColors = ListPlayerManager.instance.GetAvailableColors();
        RpcColorToOpenChosseColor(connectionToClient, availableColors);
    }

    [Command]
    public void SetColor(byte color)
    {
        this.color = color;
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
        var playerDataArray = ListPlayerManager.instance.players.ToArray();
        RpcPlayerListUI(connectionToClient, playerDataArray);
    }

    // Render list User in room
    [TargetRpc]
    private void RpcPlayerListUI(NetworkConnectionToClient connectionToClient, PlayerData[] players)
    {
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
}
