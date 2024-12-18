using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Tooltip("Card orgnization in front of camera")]
    [SerializeField] private CardOrgnization cardOrgnization;
    [SerializeField] private GameObject cardPref;
    [Header("Config animation fly tile card")]
    [SerializeField] private float durationCardFlying = 3f;
    [SerializeField] private float cardStartRotateAt = 2f;
    [SerializeField] private float cardSpawnInterval  = .2f;
    [SerializeField] private AnimationCurve easingCurve;
    [Header("Path of tile card fly")]
    [SerializeField] private List<PathPointGroup> listPathPoint = new List<PathPointGroup>();
    [SerializeField] private List<PathPointGroup> listPathPlayerToDesk = new List<PathPointGroup>();

    private List<Vector3> PathPlayerToDesk = new List<Vector3>();
    private List<Vector3> PathToPlayer = new List<Vector3>();
    
    private void Start() 
    {
        GameMaster.instance.deskCard = this;
        EventBus.Subscribe<JoinRoomEvent>(InitPath);
    }

    private void InitPath(JoinRoomEvent data)
    {
        int indexPlayer = data.index;
        foreach(Transform point in listPathPoint[indexPlayer].points)
        {
            PathToPlayer.Add(point.position);
        }
        foreach(Transform point in listPathPlayerToDesk[indexPlayer].points)
        {
            PathPlayerToDesk.Add(point.position);
        }
    }

    // Trigger discard to player
    public void DiscardToPlayer(List<int> tiles)
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

                tilecard.FlyReturnToDeskCard(currentPath, durationCardFlying, cardStartRotateAt, easingCurve, transform);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator SpawnCards(List<int> tiles)
    {
        int numberCard = tiles.Count;
        List<Vector3> listPosCard = cardOrgnization.GetListPosCard(numberCard);
        tileCardChose.TileCards.Clear();

        for (int cardIndex = 0; cardIndex < numberCard; cardIndex++)
        {
            List<Vector3> currentPath = new List<Vector3>(PathToPlayer);
            currentPath.Add(listPosCard[cardIndex]);

            GameObject card = Instantiate(cardPref);

            card.GetComponent<TileCard>().SetNumber((byte)(tiles[cardIndex]));
            TileCard tileCard = card.GetComponent<TileCard>();
            tileCardChose.TileCards.Add(tileCard);
            tileCard.FlyToPlayerView(
                currentPath, durationCardFlying, cardStartRotateAt, easingCurve, cardOrgnization.transform
            );
            yield return new WaitForSeconds(cardSpawnInterval);
        }
    }

    public void ConfirmCardChose()
    {
        if (tileCardChose.IsEnoughCardChose())
        {
            var result = tileCardChose.TileCards
            .Select(tile => new Dictionary<string, object>
            {
                { "tile", tile.number },
                { "isChosse", tile.status == CardStatus.CHOSSEN }
            })
            .ToList();
            // List<TileCardReturnServer> result = new List<TileCardReturnServer>();
            // foreach(TileCard tile in tileCardChose.TileCards)
            // {
            //     result.Add(new TileCardReturnServer(tile.number, tile.status == CardStatus.CHOSSEN? true : false));
            // }
            
            ReturnCardToDesk();

            RoomController.room.Send(MessageClientToServerGame.confirm_tile_card.ToString(), new { cards = result});
        }
        else
            GamePopupManager.Toast("Not enough");
    }
}
