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
    private List<NetworkConnection> _playerConnections =>  RoomServerManager.instance.playerConnections;

    public List<byte[]> tileSpawnMark = new List<byte[]>();

    void Awake()
    {
        _instance = this;
    }

    [Server]
    public void SpawnPlayerSlot()
    {
        List<PlayerData> playerDatas = RoomServerManager.instance.players;
        for (int i = 0; i < playerDatas.Count; i++)
        {
            GameObject playerSlot = Instantiate(playerSlotPref, listPosPlayerSlot[i].position, listPosPlayerSlot[i].rotation);
            NetworkServer.Spawn(playerSlot, _playerConnections[i]);
            spawnedPlayerSlots.Add(playerSlot);
        }
    }

   [Server]
    public void SpawnStoreCard()
    {
        StartCoroutine(SpawnStoreCardCoroutine());
    }

    [Server]
    public void SpawnMark()
    {
        StartCoroutine(SpawnMarkCoroutine());
    }

    [Server]
    private IEnumerator SpawnStoreCardCoroutine()
    {
        List<byte[]> cardData = RoomServerManager.instance.room.DistributeStoreCard(_playerConnections.Count);

        int playerIndex = 0;

        foreach (byte[] cardPlayers in cardData)
        {
             for (int i = 0; i < cardPlayers.Length; i++)
            {
                PlayerSlot playerSlot = spawnedPlayerSlots[playerIndex].GetComponent<PlayerSlot>();

                GameObject cardGameObject = Instantiate(storeCardPref, PosSpawnStoreCard.position, spawnedPlayerSlots[playerIndex].transform.rotation);
                NetworkServer.Spawn(cardGameObject, _playerConnections[playerIndex]); 


                StoreCard cardScript = cardGameObject.GetComponent<StoreCard>(); 

                cardScript.SetData(cardPlayers[i], playerIndex);   


                List<Vector3> targetPositions = playerSlot.GetPosStoreCard(cardPlayers.Length);
                Vector3 targetPosition = targetPositions[i];
                cardScript.MoveToTarget(targetPosition);

                yield return new WaitForSeconds(0.2f);
            }

            playerIndex++;
        }
    }


    private IEnumerator SpawnMarkCoroutine()
    {
        List<NetworkConnection> playerConnections = RoomServerManager.instance.playerConnections;
        yield return new WaitForSeconds(0.2f);
    }
}
