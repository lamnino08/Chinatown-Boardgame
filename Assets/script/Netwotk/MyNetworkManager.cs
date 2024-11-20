using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);

        Debug.Log($"[Server] Client disconnected: {conn.connectionId}");

        var player = conn.identity.GetComponent<PlayerManager>();
        if (player != null)
        {
            ListPlayerManager.instance.RemovePlayer(player.playerId);
        }
    }

    // [ClientRpc]
    // private void RpcNotifyPlayerLeft(string playerName)
    // {
    //     LobbyPopupManager.instance.Toast($"Player {playerName} has left the game!");
    // }
}
