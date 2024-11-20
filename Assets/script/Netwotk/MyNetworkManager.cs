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
        PlayerManager.host.CmdOnStopClient();
    }
}
