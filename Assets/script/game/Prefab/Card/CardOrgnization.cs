using System.Collections.Generic;
using UnityEngine;

public class CardOrgnization : MonoBehaviour
{
    [SerializeField] private Transform cameraa; 
    [SerializeField] private float distanceCard;
    private Vector3 pos;
    private Vector3 camPos;

    public List<Vector3> GetListPosCard(int numberCard)
    {
        pos = transform.position;
        camPos = cameraa.position;
        List<Vector3> list = new List<Vector3>();
        Vector3 lineVec = Vector3.Normalize(Vector3.Cross((camPos - pos), new Vector3(0,1,0)));

        Debug.Log(lineVec);
        Vector3 startPos = pos - ((distanceCard * (numberCard - 1))/2) * lineVec;
        for (int i = 0; i < numberCard; i++)
        {
            Vector3 posCard = startPos + lineVec * (i * distanceCard);
            list.Add(posCard);
        }
        return list;
    }
}
