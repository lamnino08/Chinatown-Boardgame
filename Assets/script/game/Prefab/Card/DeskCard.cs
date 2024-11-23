using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskCard : MonoBehaviour
{
    [SerializeField] CardOrgnization cardOrgnization;
    [SerializeField] GameObject cardPref;
    [SerializeField] Transform cardDirectionOnUI;
    [SerializeField] List<Transform> listPathPoint;
    [SerializeField] float durationCardFlying = 3f;
    [SerializeField] float cardStartRotateAt = 2f;
    [SerializeField] float cardSpawnInterval  = .2f;
    [SerializeField] AnimationCurve easingCurve;

    private List<Vector3> listPointVector3 = new List<Vector3>();
    
    private void Start() 
    {
        GameMaster.instance.deskCard = this;
        foreach(Transform point in listPathPoint)
        {
            listPointVector3.Add(point.position);
        }
    }

    // Trigger discard to player
    public void DiscardToPlayer(byte[] tiles)
    {
        StartCoroutine(SpawnCards(tiles));
    }

    private IEnumerator SpawnCards(byte[] tiles)
    {
        int numberCard = tiles.Length;
        List<Vector3> listPosCard = cardOrgnization.GetListPosCard(numberCard);

        for (int cardIndex = 0; cardIndex < numberCard; cardIndex++)
        {
            List<Vector3> currentPath = new List<Vector3>(listPointVector3);

            currentPath.Add(listPosCard[cardIndex]);

            GameObject card = Instantiate(cardPref);

            card.GetComponent<Card>().SetNumber(tiles[cardIndex]);
            CardFly cardFly = card.GetComponent<CardFly>();
            cardFly.StartFlying(
                currentPath, 
                durationCardFlying,
                cardStartRotateAt,
                easingCurve,
                cardDirectionOnUI
            );

            yield return new WaitForSeconds(cardSpawnInterval);
        }
    }

}