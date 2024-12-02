using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
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

[Serializable]
public class TileCardReturnServer
{
    public byte tile;
    public bool isChosse;
    public TileCardReturnServer()
    {
        tile = 0;
        isChosse = false;
    }
    public TileCardReturnServer(byte tile, bool isChosse)
    {
        this.tile = tile;
        this.isChosse = isChosse;
    }
}

[Serializable]
public class PathPointGroup
{
    public Transform[] points; 
}

public class DeskCard : MonoBehaviour
{
    public static TileCardChose tileCardChose = new TileCardChose();

    [SerializeField] private CardOrgnization cardOrgnization;
    [SerializeField] private GameObject cardPref;
    [SerializeField] private Transform cardDirectionOnUI;
    [SerializeField] private float durationCardFlying = 3f;
    [SerializeField] private float cardStartRotateAt = 2f;
    [SerializeField] private float cardSpawnInterval  = .2f;
    [SerializeField] private AnimationCurve easingCurve;
    [SerializeField] private List<PathPointGroup> listPathPoint = new List<PathPointGroup>();
    [SerializeField] private List<PathPointGroup> listPathPlayerToDesk = new List<PathPointGroup>();

    private List<Vector3> PathPlayerToDesk = new List<Vector3>();
    private List<Vector3> PathToPlayer = new List<Vector3>();
    
    private void Start() 
    {
        GameMaster.instance.deskCard = this;
    }

    // Trigger discard to player
    public void DiscardToPlayer(byte[] tiles)
    {
        int indexPlayer = GameMaster.localPlayer.index;
        Debug.Log(indexPlayer);
        foreach(Transform point in listPathPoint[indexPlayer].points)
        {
            PathToPlayer.Add(point.position);
        }
        foreach(Transform point in listPathPlayerToDesk[indexPlayer].points)
        {
            PathPlayerToDesk.Add(point.position);
        }
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
            List<TileCardReturnServer> result = new List<TileCardReturnServer>();
            foreach(TileCard tile in tileCardChose.TileCards)
            {
                result.Add(new TileCardReturnServer(tile.number, tile.status == CardStatus.CHOSSEN? true : false));
            }
            ReturnCardToDesk();
            GameMaster.localPlayer.ConfirmTileCard(result);
        }
        else
            GamePopupManager.Toast("Not enough");
    }
}
