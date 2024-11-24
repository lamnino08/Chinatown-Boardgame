using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileCardChose
{
    public List<TileCard> TileCards { get; private set; } = new List<TileCard>();

    public bool IsEnoughCardChose()
    {
        int countChosenCard = 0;
        foreach(TileCard tile in TileCards)
        {
            if (tile.status == CardStatus.CHOSSEN) 
            countChosenCard++;
        }
        return countChosenCard == TileCards.Count - 2;
    }
}

public class DeskCard : MonoBehaviour
{
    public static TileCardChose tileCardChose = new TileCardChose();

    [SerializeField] CardOrgnization cardOrgnization;
    [SerializeField] GameObject cardPref;
    [SerializeField] Transform cardDirectionOnUI;
    [SerializeField] float durationCardFlying = 3f;
    [SerializeField] float cardStartRotateAt = 2f;
    [SerializeField] float cardSpawnInterval  = .2f;
    [SerializeField] AnimationCurve easingCurve;
    [SerializeField] List<Transform> listPathPoint;
    [SerializeField] List<Transform> listPathPlayerToDesk;

    private List<Vector3> PathPlayerToDesk = new List<Vector3>();
    private List<Vector3> PathToPlayer = new List<Vector3>();
    
    private void Start() 
    {
        GameMaster.instance.deskCard = this;
        foreach(Transform point in listPathPoint)
        {
            PathToPlayer.Add(point.position);
        }
        foreach(Transform point in listPathPlayerToDesk)
        {
            PathPlayerToDesk.Add(point.position);
        }
    }

    // Trigger discard to player
    public void DiscardToPlayer(byte[] tiles)
    {
        StartCoroutine(SpawnCards(tiles));
    }

    public void ReturnCardToDesk()
    {
        StartCoroutine(ReturmIEnum());
    }

    private IEnumerator ReturmIEnum()
    {
        foreach(TileCard tilecard in tileCardChose.TileCards)
        {
            if (tilecard.status == CardStatus.CHOSSING)
            {
                List<Vector3> currentPath = new List<Vector3>(PathPlayerToDesk);
                currentPath[0] = tilecard.transform.position;

                CardFly cardFly = tilecard.cardFly;
                cardFly.StartFlying(currentPath, durationCardFlying, cardStartRotateAt, easingCurve, transform, CardStatus.LYING);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator SpawnCards(byte[] tiles)
    {
        int numberCard = tiles.Length;
        List<Vector3> listPosCard = cardOrgnization.GetListPosCard(numberCard);
        tileCardChose.TileCards.Clear();

        for (int cardIndex = 0; cardIndex < numberCard; cardIndex++)
        {
            List<Vector3> currentPath = new List<Vector3>(PathToPlayer);

            currentPath.Add(listPosCard[cardIndex]);

            GameObject card = Instantiate(cardPref);

            card.GetComponent<TileCard>().SetNumber((byte)(tiles[cardIndex]));
            CardFly cardFly = card.GetComponent<CardFly>();
            tileCardChose.TileCards.Add(cardFly.card);
            cardFly.StartFlying(
                currentPath, 
                durationCardFlying,
                cardStartRotateAt,
                easingCurve,
                cardDirectionOnUI,
                CardStatus.CHOSSING
            );

            yield return new WaitForSeconds(cardSpawnInterval);
        }
    }

    public void ConfirmCardChose()
    {
        if (tileCardChose.IsEnoughCardChose())
        {
            ReturnCardToDesk();
        }
        else
            GamePopupManager.Toast("Not enough");
    }

    
}