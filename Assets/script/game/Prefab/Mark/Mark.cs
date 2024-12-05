using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MarkStatus
{
    FREE, // free 
    ONTILE, // it mark to tile
    DRAGING, // draging
    FLYING  // flying
}
public class Mark : PieceGameObject
{
    private MarkStatus _status;
    public MarkStatus status 
    {
        get
        {
            if (movement.isFlying) return MarkStatus.FLYING;
            return _status;
        }
    }

    [SerializeField] private MarkAppearance appearance;
    [SerializeField] private MarkMovement movement;
    
    private int _owner;

    public override void OnStartClient()
    {
        gameObject.layer = LayerMask.NameToLayer("Mark");
    }

    #region Command

    #endregion Command

    #region Server 
    [Server]
    public void SetData(int indexPlayerOwner, byte color)
    {
        _owner = indexPlayerOwner;
        RpcSetColor(color);
    }

    [Server]
    public void MoveToTile(
        Vector3 bowPos, Vector3 tilePos
    )
    {
        RpcMoveToTile(bowPos, tilePos);
    }
#endregion Server

#region Client RPC
    [ClientRpc]
    private void RpcSetColor(byte color)
    {
        appearance.SetColor(color);
    }

    [ClientRpc]
    public void RpcMoveToTile(
        Vector3 bowPos, Vector3 tilePos
    )
    {
        movement.StartFlyingSimpleCurve(bowPos, tilePos, 0.7f); 
        _status = MarkStatus.ONTILE;
    }
#endregion ClientRPC

#region Client
    public override void OnMouseClick()
    {
        Debug.Log("here");
        GamePopupManager.Toast($"Mark {_owner} click");
        if (!isOwned) GamePopupManager.Toast("It's not your mark"); // do nothing if has no authority
        if (status == MarkStatus.FLYING) return; // do nothing if mark is flying

        GameMaster.gameManager.OnMarkClick(this);
    }

    public void Click()
    {
        appearance.Highlight(true);
    }

    public void UnClick()
    {
        // Set UI or somthing
        appearance.Highlight(false);
    }   
#endregion Client
}
