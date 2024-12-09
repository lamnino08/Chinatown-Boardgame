using Mirror;
using UnityEngine;

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
    [SerializeField] private LayerMask tileLayer;
    
    private int _owner;

    public override void OnStartClient()
    {
        gameObject.layer = LayerMask.NameToLayer("Mark");
    }

    #region Command
    [Command]
    private void CmdHighlight(bool isHighlight)
    {
        RpcHightlight(isHighlight);
    }

    [Command]
    public void CmdMoveToTile(int tileindex)
    {
        Tile tile = Map.instance.GetTile(tileindex);
        
        // unmark old tile
        Tile oledTile = GetTileOn();
        if (oledTile != null)
        {
            oledTile.UnMark();
        }

        // Move to new Tile
        SVMoveToTile(transform.position, tile);
    }

    [Command]
    public void CmdMoveToTable(Vector3 pos)
    {
        //Un mark
        Tile oledTile = GetTileOn();
        if (oledTile != null)
        {
            oledTile.UnMark();
        }
        RpcMoveToTarget(pos);
    }
    #endregion Command

    #region Server 
    [Server]
    public void SetData(int indexPlayerOwner, byte color)
    {
        _owner = indexPlayerOwner;
        RpcSetColor(color);
    }

    [Server]
    public void SVMoveToTile(
        Vector3 bowPos, Tile tile
    )
    {
        RpcMoveToTile(bowPos, tile.transform.position + new Vector3(0,.2f, 0));
        tile.SetOwner(_owner);
    }

    [Server]
    private Tile GetTileOn()
    {
        Vector3 direction = -Vector3.up;

        // Perform the raycast
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 1f, tileLayer))
        {
            Tile mark = hit.collider.GetComponent<Tile>();
            return mark; 
        }

        GamePopupManager.Toast("No Tile object found in the upward direction.");
        return null;
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

    [ClientRpc]
    public void RpcMoveToTarget(
        Vector3 targetPos
    )
    {
        movement.StartFlyingSimpleCurve(transform.position, targetPos, 0.7f); 
        appearance.Highlight(false);
        _status = MarkStatus.FREE;
    }

    [ClientRpc]
    private void RpcHightlight(bool isHighlight)
    {
        appearance.Highlight(isHighlight);
    }
#endregion ClientRPC

#region Client
    [Client]
    public override void OnMouseClick()
    {
        Debug.Log($"Mark {_owner} click");
        if (!isOwned) Debug.Log("It's not your mark"); // do nothing if has no authority
        if (status == MarkStatus.FLYING) return; // do nothing if mark is flying

        GameMaster.gameManager.OnMarkClick(this);
    }

    [Client]
    public void Click()
    {
        CmdHighlight(true);
    }

    [Client]
    public void UnClick()
    {
        CmdHighlight(false);
    }   
#endregion Client
}
