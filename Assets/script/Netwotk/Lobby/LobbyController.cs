using System;
using System.Collections.Generic;
using Colyseus;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    public static ColyseusRoom<Lobby> lobby { get; private set; }

    public static Lobby state;

    public void Init(ColyseusRoom<Lobby> lobby)
    {
        LobbyController.lobby = lobby;
        state = lobby.State;
        lobby.State.players.OnAdd(OnAddPlayer);
        lobby.State.players.OnRemove(OnRemovePlayer);

        RegisterAllMessages(); // Registe trigger all message from server
    }

    private void RegisterAllMessages()
    {
        foreach (MessageServerToClientLobby messageType in System.Enum.GetValues(typeof(MessageServerToClientLobby)))
        {
            lobby.OnMessage<Dictionary<string, object>>(messageType.ToString(), (data) =>
            {
                Debug.Log($"On message {messageType.ToString()}");
                EventHandlerNetwork.Trigger(messageType.ToString(), data);
            });
        }
    }

    private void OnAddPlayer(string key, PlayerLobby player)
    {
        LobbyUIManager.instance.AddNewPlayer(player);
    }

    private void OnRemovePlayer(string key, PlayerLobby player)
    {
        Debug.Log($"Player removed: {player.name} (Key: {key})");
        LobbyUIManager.instance.RemovePlayer(player);
    }
}
