using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Collections;

public class GameServerManager : NetworkBehaviour 
{
    private static GameServerManager _instance;
    public static GameServerManager instance => _instance;

    [Header("Player slot")]
    [SerializeField] public List<Transform> listPosPlayerSlot;
    [SerializeField] public GameObject playerSlotPref;

    [Header("Store Card")]
    [SerializeField] private StoreCardDesk storeCardDesk;

    private List<GameObject> _spawnedPlayerSlots = new List<GameObject>();
    public List<GameObject> spawnedPlayerSlots =>_spawnedPlayerSlots;
    private List<NetworkConnection> _playerConnections =>  RoomServerManager.instance.playerConnections;

    public List<byte[]> tileSpawnMarkSave = new List<byte[]>();

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
            _spawnedPlayerSlots.Add(playerSlot);
        }
    }

    public void SpawnStoreCard()
    {
        storeCardDesk.SpawnStoreCard();
    }
   

    [Server]
    public void SpawnMark()
    {
        StartCoroutine(SpawnMarkCoroutine());
    }

   

    private IEnumerator SpawnMarkCoroutine()
    {
        for(int indexPlayer = 0; indexPlayer < _playerConnections.Count; indexPlayer++)
        {
            for(int tile = 0; tile < tileSpawnMarkSave[indexPlayer].Length; tile++)
            {

            }
        }
        yield return new WaitForSeconds(0.2f);
    }
}
