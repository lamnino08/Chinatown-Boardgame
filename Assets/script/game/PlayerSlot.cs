using System.Collections.Generic;
using UnityEngine;

public class PlayerSlot : MonoBehaviour
{
    [SerializeField] private Transform storeCardContain; // Gốc để tính toán
    [SerializeField] private float spacing = 0.6f; // Khoảng cách giữa các card

    /// <summary>
    /// Lấy danh sách vị trí cho các StoreCard dựa trên hướng của storeCardContain
    /// </summary>
    /// <param name="numberOfCards">Số lượng card</param>
    /// <returns>Danh sách các vị trí</returns>
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
