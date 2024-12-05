using Mirror;
using UnityEngine;

[RequireComponent(typeof(StoreCardMovement), typeof(StoreCardAppearance))]
public class StoreCard : PieceGameObject
{
    [SerializeField] public StoreCardAppearance appearance;
    [SerializeField] public StoreCardMovement movement;
    private int _ownerIndex;

    public override void OnStartClient()
    {
        gameObject.layer = LayerMask.NameToLayer("StoreCard");
    }

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

    
    #endregion ClientRPC

#region Client
    public override void OnMouseClick()
    {
        // throw new System.NotImplementedException();
        Debug.Log("StoreCard click");
    }
#endregion Client
}
