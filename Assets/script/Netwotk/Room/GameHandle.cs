using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class GameHandle : MonoBehaviour 
{
    private void Awake() {
        foreach (MessageServerToClientGame messageType in Enum.GetValues(typeof(MessageServerToClientGame)))
        {
            string methodName = $"NW_{messageType}";
            string messageString = messageType.ToString();

            MethodInfo method = GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (method != null)
            {
                EventHandlerNetwork.Register(messageString, (data) =>
                {
                    method.Invoke(this, new object[] { data });
                });

                Debug.Log($"Registered handler for {messageType} -> {methodName} with message type: {messageString}");
            }        

            else
            {
                Debug.LogWarning($"No handler found for {messageType} -> {methodName}");
            }
        }
    }

    // Hàm xử lý cho PlayerChooseColor
    private void NW_tile_cards(object data)
    {
        var messageData = data as Dictionary<string, object>;

        var cards = messageData["cards"] as List<object>;
        
        if (cards != null)
        {
            List<int> cardsIndex = cards
            .Select(card => Convert.ToInt32(card))
            .ToList();
            GameMaster.instance.deskCard.DiscardToPlayer(cardsIndex);
            GameUIManager.instance.ReceiveCardDiscard();
        }
        else
        {
            Debug.LogError("Failed to parse 'cards' from message data.");
        }
    }

    private void NW_player_done_deal_tile_card(object data) 
    {
        var messageData = data as Dictionary<string, object>;

        string sessionId = messageData["sessionId"].ToString();
        var cards = messageData["cards"] as List<object>;

        if (cards != null)
        {
            List<int> cardsIndex = cards
            .Select(card => Convert.ToInt32(card))
            .ToList();

            Debug.Log("On Player done deal card");
            EventBus.Notificate(new PlayerDoneDealCardEvent(sessionId, cardsIndex));
        }
        else
        {
            Debug.LogError("Failed to parse 'cards' from message data.");
        }
    }

    private void NW_all_done_deal_tile_card(object data)
    {
        var messageData = data as Dictionary<string, object>;

        if (messageData.TryGetValue("liststoreCards", out var rawCards))
        {
            Debug.Log("On all player done deal tile card");
            var cards = rawCards as List<object>;
            
            List<int[]> cardsIndex = cards
                .Select(cardObj =>
                {
                    var cardList = cardObj as List<object>;
                    if (cardList != null)
                    {
                        return cardList.Select(card => Convert.ToInt32(card)).ToArray();
                    }
                    return null;
                })
                .ToList();
            EventBus.Notificate(new AllDoneDealCardEvent(cardsIndex));
        }
        else
        {
            Debug.LogError("Message data is null or missing 'liststoreCards'.");
        }
    }
}
