using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class StoreCardDesk : NetworkBehaviour
{
    [SerializeField] private GameObject storeCardPref;
    private List<GameObject> spawnedPlayerSlots => GameServerManager.instance.spawnedPlayerSlots;
    private List<NetworkConnection> _playerConnections =>  RoomServerManager.instance.playerConnections;

    public override void OnStartServer()
    {
        base.OnStartServer();
        EventBus.Subscribe<SpawnStoreCardEvent>(SpawnStoreCard);
    }

    [Server]
    private void SpawnStoreCard(SpawnStoreCardEvent data)
    {
        IReadOnlyList<byte[]> cardData = data.storeCards;
        StartCoroutine(SpawnStoreCardCoroutine(cardData));
    }

    [Server]
    private IEnumerator SpawnStoreCardCoroutine(IReadOnlyList<byte[]> cardData)
    {   
        int playerIndex = 0;

        foreach (byte[] cardPlayers in cardData)
        {
             for (int i = 0; i < cardPlayers.Length; i++)
            {
                PlayerSlot playerSlot = spawnedPlayerSlots[playerIndex].GetComponent<PlayerSlot>();

                GameObject cardGameObject = Instantiate(storeCardPref, transform.position, spawnedPlayerSlots[playerIndex].transform.rotation);
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

}
