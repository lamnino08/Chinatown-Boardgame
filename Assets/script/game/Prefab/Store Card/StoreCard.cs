using Mirror;
using UnityEngine;

[RequireComponent(typeof(StoreCardMovement), typeof(StoreCardAppearance))]
public class StoreCard : NetworkBehaviour
{
    [SerializeField] public StoreCardAppearance appearance;
    [SerializeField] public StoreCardMovement movement;
    private int _ownerIndex;

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
}
