using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Mark : NetworkBehaviour
{
    [SerializeField] private MarkAppearance appearance;
    [SerializeField] private MarkMovement movement;
    
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
        Vector3 bowPos, Vector3 tilePos
    )
    {
        RpcMoveToTile(bowPos, tilePos);
    }

    [ClientRpc]
    public void RpcMoveToTile(
        Vector3 bowPos, Vector3 tilePos
    )
    {
        movement.StartFlyingSimpleCurve(bowPos, tilePos, 0.7f); 
    }
}
