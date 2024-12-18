using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class StoreCardDesk : MonoBehaviour
{
    [SerializeField] private GameObject storeCardPref;
    public List<GameObject> spawnedPlayerSlots => GameManager.instance.spawnedPlayerSlots;
    
    public void Start()
    {
        EventBus.Subscribe<AllDoneDealCardEvent>(SpawnStoreCard);
    }

    private void SpawnStoreCard(AllDoneDealCardEvent data)
    {
        StartCoroutine(SpawnStoreCardCoroutine(data.storeCards));
    }

    private IEnumerator SpawnStoreCardCoroutine(List<int[]> cardData)
    {   
        int playerIndex = 0;

        foreach (int[] playerCards in cardData)
        {
             for (int i = 0; i < playerCards.Length; i++)
            {
                PlayerSlot playerSlot = spawnedPlayerSlots[playerIndex].GetComponent<PlayerSlot>();

                GameObject cardGameObject = Instantiate(storeCardPref, transform.position, spawnedPlayerSlots[playerIndex].transform.rotation);

                //Set data for store cards
                StoreCard cardScript = cardGameObject.GetComponent<StoreCard>(); 
                cardScript.SetData(playerCards[i], playerIndex, playerSlot.sessionId);   

                // Move store card to player slot
                List<Vector3> targetPositions = playerSlot.GetPosStoreCard(playerCards.Length);
                Vector3 targetPosition = targetPositions[i];
                cardScript.MoveToTarget(targetPosition);

                yield return new WaitForSeconds(0.2f);
            }

            playerIndex++;
        }
    }

}
