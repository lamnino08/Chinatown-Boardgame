using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerSlot : NetworkBehaviour
{
    public static PlayerSlot localPlayerSlot;
    [SerializeField] private Transform _storeCardContain;
    [SerializeField] private float _spacing = 0.32f;
    [SerializeField] private MarkBowl _markBowl;
    [SerializeField] private Transform _cardHole;
    private NetworkConnection ownConnect;

    [SyncVar]
    private int _index = 0;
    private byte _color  = 0;

    void Start()
    {
        EventBus.Subscribe<SpawnMarkEvent>(OnSpawnkMarkDistribute);
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        localPlayerSlot = this;
    }

    [Command]
    public void StoreConnectionOwner()
    {
        ownConnect = connectionToClient;
    }

    [Command]
    public void HightLightTile(byte tile, bool isHighlight)
    {
        Map.instance.HightLightTile(connectionToClient, tile, isHighlight);   
    }

    [Server]
    public void SetData(int index, byte color)
    {
        this._index = index;
        this._color = color;
    }

    [Server]
    private void OnSpawnkMarkDistribute(SpawnMarkEvent tiles)
    {
        IReadOnlyList<byte[]> tilesData = tiles.tiles;
        _markBowl.SpawnMark(tilesData[_index], _color, _index);
        TRpcDistributeCard(ownConnect);
    }

    [Server]
    public List<Vector3> GetPosStoreCard(int numberCard)
    {
        List<Vector3> list = new List<Vector3>();
        Vector3 lineVec = _storeCardContain.right;

        Vector3 startPos = _storeCardContain.position - (_spacing *( (numberCard)/2 - 0.5f) + (numberCard % 2)/2) * lineVec;
        for (int i = 0; i < numberCard; i++)
        {
            Vector3 posCard = startPos + lineVec * (i * _spacing) + new Vector3(0, 0.25f, 0);
            list.Add(posCard);
        }
        return list;
    }

    [TargetRpc]
    public void TRpcDistributeCard(NetworkConnection conn)
    {
        EventBus.Notificate(new TileCardToHoleEvent(_cardHole));
    }
}
