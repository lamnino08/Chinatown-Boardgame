using Mirror;
using Telepathy;
using UnityEngine;

public enum StoreCardSatus
{
    FREE,
    ONTILE,
    FLYING
}
[RequireComponent(typeof(StoreCardMovement), typeof(StoreCardAppearance))]
public class StoreCard : PieceGameObject
{
    [SerializeField] public StoreCardAppearance appearance;
    [SerializeField] public StoreCardMovement movement;
    [SerializeField] public HighLight _highlight;
    
    private int _ownerIndex;
    private StoreCardSatus _status;
    public StoreCardSatus status 
    {
        get
        {
            if (movement.isFlying) return StoreCardSatus.FLYING;
            return _status;
        }
    }
    
    public override void OnStartClient()
    {
        gameObject.layer = LayerMask.NameToLayer("StoreCard");
    }

#region Command
    [Command]
    public void CmdMoveToTaget(Vector3 pos)
    {
        //Un mark
        RpcMoveToTarget(pos);
    }

    [Command]
    public void CmdHighlgith(bool isHighlight, byte color)
    {
        RpcHighlight(isHighlight, color);
    }
#endregion Command

#region Server
    [Server]
    public void SetData(byte storeCard, int owner)
    {
        RpcSetImageStoreCard(storeCard);
        _ownerIndex = owner;
    }

    [Server]
    public void MoveToTarget(Vector3 target)
    {   
        RpcMoveToTaget(target);
    }
#endregion Server

#region ClientRPC
    [ClientRpc]
    private void RpcMoveToTaget(Vector3 target)
    {
        movement.MoveToTarget(target);
    }

    [ClientRpc]
    private void RpcSetImageStoreCard(byte storeCard)
    {
        appearance.SetImage(storeCard);
    }

    [ClientRpc]
    public void RpcMoveToTarget(
        Vector3 targetPos
    )
    {
        movement.MoveToTarget(targetPos); 
        _status = StoreCardSatus.FREE;
    }

    [ClientRpc]
    private void RpcHighlight(bool isHighlight, byte color)
    {
        _highlight.ToggleHighlight(true);
    }
#endregion ClientRPC

#region Client
    public override void OnMouseClick()
    {
        if (!isOwned) GamePopupManager.Toast("It's not your card"); // do nothing if has no authority
        if (status == StoreCardSatus.FLYING) return;

        GameMaster.gameManager.OnStoreClick(this);
    }

    [Client]
    public void Highlgith(bool isHighlight, byte color)
    {
        _highlight.ToggleHighlight(true, GameMaster.localPlayer.color);
    }
#endregion Client
}
