using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Mirror;
using UnityEngine;

public class Mark : NetworkBehaviour
{
    [SerializeField] private MarkAppearance appearance;
    [SerializeField] private MarkFly movement;
    
    private int owner;
    
    [Server]
    public void SetData(int indexPlayerOwner, byte color)
    {
        owner = indexPlayerOwner;
        RpcSetColor(color);
    }

    [ClientRpc]
    private void RpcSetColor(byte color)
    {
        appearance.SetColor(color);
    }

    [Server]
    public void MoveToTile(
        List<Vector3> controlPoints
    )
    {
        RpcMoveToTile(controlPoints);
    }

    [ClientRpc]
    public void RpcMoveToTile(
        List<Vector3> controlPoints
    )
    {
        movement.StartFlying(controlPoints); 
    }
}
