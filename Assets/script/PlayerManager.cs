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

    public override void OnStopClient()
    {
        ListPlayerManager.Instance.RemovePlayer(playerId);
        // Xóa UI của người chơi khi client rời
        RpcRemoveUserUI(playerName);
    }

    [Command]
    private void CmdSetPlayerData(string name)
    {
        ulong randomID = (ulong)System.DateTime.Now.Ticks;
        playerId = randomID;
        playerName = name;

        Player newPlayer = new Player(randomID, name );

        ListPlayerManager.Instance.AddPlayer(playerId, playerName);
        RpcAddUserUI(playerName);
    }

    private void OnNameChanged(string oldName, string newName)
    {
        Debug.Log($"Player name updated: {newName}");
    }


    [ClientRpc]
    private void RpcAddUserUI(string name)
    {
        Debug.Log(name);
    }

    [ClientRpc]
    private void RpcRemoveUserUI(string name)
    {
        Debug.Log($"Removing player from UI: {name}");
        // Implement logic to remove the player's UI element
    }
}
