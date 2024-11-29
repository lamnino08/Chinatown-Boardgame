using System.Collections.Generic;
using UnityEngine;

public class PlayerSlot : MonoBehaviour
{
    [SerializeField] private Transform storeCardContain;
    [SerializeField] private float spacing = 0.6f;
    [SerializeField] private MarkBowl markBowl;

    private int _index = 0;
    private byte _color  = 0;

    void Start()
    {
        EventBus.Subscribe<SpawnMarkEvent>(OnSpawnkMarkDistribute);
    }

    public void SetData(int index)
    {
        this._index = index;
    }

    private void OnSpawnkMarkDistribute(SpawnMarkEvent tiles)
    {
        IReadOnlyList<byte[]> tilesData = tiles.tiles;
        Color color = Util.TransferColor(_color); // Color of mark spawn
        markBowl.SpawnMark(tilesData[_index], color);
    }

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


}
