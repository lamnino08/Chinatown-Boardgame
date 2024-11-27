using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Collections;

public class GameServerManager : NetworkBehaviour 
{
    private static GameServerManager _instance;
    public static GameServerManager instance => _instance;

    [SerializeField] public List<Transform> listPosPlayerSlot;
    [SerializeField] public GameObject playerSlotPref;
    [SerializeField] public Transform PosSpawnStoreCard;
    [SerializeField] public GameObject storeCardPref;
    [SerializeField] public GameObject markPref;

    private List<GameObject> spawnedPlayerSlots = new List<GameObject>();

    void Awake()
    {
        _instance = this;
    }

    [Server]
    public void SpawnPlayerSlot()
    {
        List<NetworkConnection> playerConnections = RoomServerManager.instance.playerConnections;
        List<PlayerData> playerDatas = RoomServerManager.instance.players;
        for (int i = 0; i < playerDatas.Count; i++)
        {
            GameObject playerSlot = Instantiate(playerSlotPref, listPosPlayerSlot[i].position, listPosPlayerSlot[i].rotation);
            NetworkServer.Spawn(playerSlot, playerConnections[i]);
            spawnedPlayerSlots.Add(playerSlot);
        }
    }

   [Server]
    public void SpawnStoreCard()
    {
        StartCoroutine(SpawnStoreCardCoroutine());
    }

    [Server]
    private IEnumerator SpawnStoreCardCoroutine()
    {
        List<NetworkConnection> playerConnections = RoomServerManager.instance.playerConnections;
        List<byte[]> cardData = RoomServerManager.instance.room.DistributeStoreCard(playerConnections.Count);

        int playerIndex = 0; // Index để gắn card vào đúng PlayerSlot

        foreach (byte[] cardPlayers in cardData)
        {
             foreach (byte storeCard in cardPlayers)
            {
                // Tạo StoreCard
                GameObject cardGameObject = Instantiate(storeCardPref, PosSpawnStoreCard.position, storeCardPref.transform.rotation);

                // Lấy vị trí chứa card trong PlayerSlot
                PlayerSlot playerSlot = spawnedPlayerSlots[playerIndex].GetComponent<PlayerSlot>();
                List<Vector3> targetPositions = playerSlot.GetPosStoreCard(cardPlayers.Length);

                int cardIndex = System.Array.IndexOf(cardPlayers, storeCard);
                Vector3 targetPosition = targetPositions[cardIndex];

                NetworkServer.Spawn(cardGameObject);

                StoreCardMovement mover = cardGameObject.GetComponent<StoreCardMovement>();
                mover.MoveToTarget(targetPosition);

                yield return new WaitForSeconds(0.2f);
            }

            playerIndex++;
        }
    }

}
