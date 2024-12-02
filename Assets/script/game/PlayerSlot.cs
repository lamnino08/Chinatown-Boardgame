using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerSlot : NetworkBehaviour
{
    public static PlayerSlot localPlayerSlot;
    [SerializeField] private Transform storeCardContain;
    [SerializeField] private float spacing = 0.6f;
    [SerializeField] private MarkBowl markBowl;

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
        Debug.Log("o");
        localPlayerSlot = this;
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
        markBowl.SpawnMark(tilesData[_index], _color, _index);
    }

    [Server]
    public List<Vector3> GetPosStoreCard(int numberCard)
    {
        List<Vector3> list = new List<Vector3>();
        Vector3 lineVec = storeCardContain.right;

        Vector3 startPos = storeCardContain.position - (spacing *( (numberCard)/2 - 0.5f) + (numberCard % 2)/2) * lineVec;
        for (int i = 0; i < numberCard; i++)
        {
            Vector3 posCard = startPos + lineVec * (i * spacing);
            list.Add(posCard);
        }
        return list;
    }

    [Command]
    public void HightLightTile(byte tile, bool isHighlight)
    {
        Map.instance.HightLightTile(connectionToClient, tile, isHighlight);   
    }
}
