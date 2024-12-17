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
}
