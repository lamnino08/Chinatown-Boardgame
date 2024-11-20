using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    [SyncVar]
    public ulong playerId;

    public override void OnStartLocalPlayer()
    {
        CmdSetPlayerData(PlayerPrefs.GetString("PlayerName"));
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

    [Server]
    private void CmdOnStopClient()
    {
        ListPlayerManager.instance.RemovePlayer(playerId);
        RpcRemoveUserUI(playerName);
    }

    private void OnNameChanged(string oldName, string newName)
    {
        Debug.Log($"Player name updated: {newName}");
    }

    [Server]
    public void GetDataInLobby(NetworkConnectionToClient  connectionToClient)
    {
        var playerDataArray = ListPlayerManager.instance.players.ToArray();
        RpcPlayerListUI(connectionToClient, playerDataArray);
    }

    [TargetRpc]
    private void RpcPlayerListUI(NetworkConnectionToClient connectionToClient, PlayerData[] players)
    {
        LobbyUIManager.instance.SetSlotPlayerUI(players);
    }

    [ClientRpc]
    private void RpcRemoveUserUI(string name)
    {
        LobbyUIManager.instance.RemovePlayer(name);
    }

    [ClientRpc]
    private void RpcNewPlayerUI(PlayerData newPlayer)
    {
        LobbyUIManager.instance.AddNewPlayerUI(newPlayer);
    }
}
